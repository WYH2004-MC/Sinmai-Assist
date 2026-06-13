using HarmonyLib;
using MAI2.Util;
using Manager;
using Manager.UserDatas;
using MelonLoader;
using Monitor;
using Process;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SinmaiAssist.Cheat;

internal class FastSkip
{
    private enum GameSequence
    {
        Init, Sync, Start, StartWait, Play, PlayEnd, Result, ResultEnd, FinalWait, Release
    }

    public static bool CustomSkip = false;
    public static bool SkipButton = false;
    public static bool Force1Miss = false;
    public static int CustomAchivement = 0;
    
    private static bool _isSkip = false;
    private static bool _Miss = false;

    private static bool MarkConnectSlideJudged(NoteData note)
    {
        if (!note.type.isConnectSlide()) return false;

        note.isJudged = true;
        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameProcess), "OnUpdate")]
    public static void Skip(GameProcess __instance)
    {
        try
        {
            System.Type processBaseType = typeof(GameProcess).BaseType;
            GameSequence _sequence = (GameSequence)typeof(GameProcess).GetField("_sequence", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
            var UpdateSubbMonitorDataMethod = typeof(GameProcess).GetMethod("UpdateSubbMonitorData", BindingFlags.NonPublic | BindingFlags.Instance);
            var SetReleaseMethod = typeof(GameProcess).GetMethod("SetRelease", BindingFlags.NonPublic | BindingFlags.Instance);
            var IsPartyPlayMethod = typeof(GameProcess).GetMethod("IsPartyPlay", BindingFlags.NonPublic | BindingFlags.Instance);
            var containerField = processBaseType.GetField("container", BindingFlags.NonPublic | BindingFlags.Instance);
            ProcessDataContainer container = (ProcessDataContainer)containerField.GetValue(__instance);
            if (_sequence >= GameSequence.Play && _sequence < GameSequence.Release && !GameManager.IsNoteCheckMode)
            {
                _isSkip = false;
                if (DebugInput.GetKeyDown(KeyCode.Space) || SkipButton)
                {
                    _isSkip = true;
                    GameMonitor[] monitors = (GameMonitor[])typeof(GameProcess).GetField("_monitors", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
                    if (CustomSkip)
                    {
                        for (int i = 0; i < monitors.Length; i++)
                        {
                            monitors[i].Seek(0);
                        }
                        NotesManager.StartPlay(0);
                        NotesManager.Pause(true);
                        bool IsPartyPlay = (bool)IsPartyPlayMethod.Invoke(__instance, null);
                        Singleton<GamePlayManager>.Instance.Initialize(IsPartyPlay);
                        uint maxCombo = 0u;
                        for (int i = 0; i < monitors.Length; i++)
                        {
                            if (Singleton<UserDataManager>.Instance.GetUserData(i).IsEntry)
                            {
                                monitors[i].ForceAchivement(CustomAchivement, 0);
                                maxCombo += Singleton<GamePlayManager>.Instance.GetGameScore(i).MaxCombo;
                            }
                        }
                        GameScoreList gameScore = Singleton<GamePlayManager>.Instance.GetGameScore(2);
                        if (gameScore.IsEnable && !gameScore.IsHuman())
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                if (Singleton<UserDataManager>.Instance.GetUserData(i).IsEntry && GameManager.SelectGhostID[i] != GhostManager.GhostTarget.End)
                                {
                                    UserGhost ghostToEnum = Singleton<GhostManager>.Instance.GetGhostToEnum(GameManager.SelectGhostID[i]);
                                    gameScore.SetForceAchivement_Battle((float)GameManager.ConvAchiveIntToDecimal(ghostToEnum.Achievement));
                                    break;
                                }
                            }
                        }
                        for (int i = 0; i < monitors.Length; i++)
                        {
                            if (Singleton<UserDataManager>.Instance.GetUserData(i).IsEntry)
                            {
                                Singleton<GamePlayManager>.Instance.GetGameScore(i).SetChain(maxCombo);
                            }
                        }
                    }
                    if (_isSkip)
                    {
                        for (int i = 0; i < monitors.Length; i++)
                        {
                            if (Singleton<UserDataManager>.Instance.GetUserData(i).IsEntry)
                            {
                                UpdateSubbMonitorDataMethod.Invoke(__instance, new object[] { i });
                                Message[] message = (Message[])typeof(GameProcess).GetField("_message", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
                                container.processManager.SendMessage(message[i]);
                                Singleton<GamePlayManager>.Instance.SetSyncResult(i);
                            }
                        }
                        SetReleaseMethod.Invoke(__instance, null);
                        SkipButton = false;
                    }
                }
            }
        }
        catch (Exception e) { MelonLogger.Error(e); }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameScoreList), "SetForceAchivement")]
    public static bool SetForceAchivement(int achivement, int dxscore, GameScoreList __instance)
    {
        _Miss = false;
        decimal targetAchive = achivement;
        if (targetAchive >= 100.0m && targetAchive <= 100.4m) targetAchive = 100.3m;

        int monitorIndex = (int)typeof(GameScoreList).GetField("_monitorIndex", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
        NoteDataList noteList = NotesManager.Instance(monitorIndex).getReader().GetNoteList();
        HashSet<string> judgedSlots = new HashSet<string>();

        // 101满分路径
        if (targetAchive >= 101.0m)
        {
            bool forcedMiss = false;
            foreach (NoteData note in noteList)
            {
                if (MarkConnectSlideJudged(note)) continue;
                if (note.indexNote < 0) continue;
                NoteScore.EScoreType st = GamePlayManager.NoteType2ScoreType(note.type);

                // 每个note的每个槽位只判定一次
                if (!judgedSlots.Add(note.indexNote + "_" + st)) continue;
                
                NoteJudge.ETiming timing = NoteJudge.ETiming.Critical;
                if (Force1Miss && !forcedMiss && st != NoteScore.EScoreType.End)
                {
                    timing = NoteJudge.ETiming.TooFast;
                    forcedMiss = true;
                    _Miss = true;
                }

                // End 类型特殊处理
                __instance.SetResult(note.indexNote, st, timing);

                // Break
                if (st == NoteScore.EScoreType.Break && timing != NoteJudge.ETiming.TooFast)
                {
                    __instance.SetResult(note.indexNote, NoteScore.EScoreType.BreakBonus, timing);
                }
            }
            return false;
        }

        // 非满分路径
        decimal factor = targetAchive / 100.0m;
        long budgetBase = (long)((decimal)__instance.ScoreTotal._allPerfectScore * (factor > 1.0m ? 1.0m : factor));
        long budgetBonus = (long)((decimal)__instance.ScoreTotal._breakBonusScore * (targetAchive > 100.0m ? (targetAchive - 100.0m) : factor));
        if (targetAchive == 100.0m) { budgetBase = __instance.ScoreTotal._allPerfectScore; budgetBonus = 0; }

        NoteJudge.ETiming[] NoteArray = new NoteJudge.ETiming[7] {
            NoteJudge.ETiming.Critical, NoteJudge.ETiming.FastGreat, NoteJudge.ETiming.FastGreat2nd,
            NoteJudge.ETiming.LateGreat, NoteJudge.ETiming.LateGreat2nd, NoteJudge.ETiming.LateGreat3rd, NoteJudge.ETiming.LateGood
        };

        foreach (NoteData note in noteList)
        {
            if (MarkConnectSlideJudged(note)) continue;
            if (note.indexNote < 0) continue;
            NoteScore.EScoreType st = GamePlayManager.NoteType2ScoreType(note.type);
            if (!judgedSlots.Add(note.indexNote + "_" + st)) continue;

            if (st == NoteScore.EScoreType.End) {
                __instance.SetResult(note.indexNote, st, NoteJudge.ETiming.Critical);
                continue;
            }

            bool flag = false;
            foreach (NoteJudge.ETiming eTiming in NoteArray) {
                long cBase = NoteScore.GetJudgeScore(eTiming, st);
                long cBonus = (st == NoteScore.EScoreType.Break) ? NoteScore.GetJudgeScore(eTiming, NoteScore.EScoreType.BreakBonus) : 0;
                if (budgetBase >= cBase && budgetBonus >= cBonus) {
                    budgetBase -= cBase; budgetBonus -= cBonus;
                    __instance.SetResult(note.indexNote, st, eTiming);
                    if (st == NoteScore.EScoreType.Break) __instance.SetResult(note.indexNote, NoteScore.EScoreType.BreakBonus, eTiming);
                    flag = true; break;
                }
            }
            if (!flag) { __instance.SetResult(note.indexNote, st, NoteJudge.ETiming.TooFast); _Miss = true; }
        }
        return false;
    }
}
