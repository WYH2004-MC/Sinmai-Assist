using HarmonyLib;
using MAI2.Util;
using Manager;
using Manager.UserDatas;
using MelonLoader;
using Monitor;
using Process;
using System;
using System.Reflection;
using UnityEngine;

namespace Cheat
{
    internal class FastSkip
    {
        private enum GameSequence
        {
            Init,
            Sync,
            Start,
            StartWait,
            Play,
            PlayEnd,
            Result,
            ResultEnd,
            FinalWait,
            Release
        }

        public static bool CustomSkip = false;
        public static bool SkipButton = false;
        public static bool IsSkip = false;
        public static int CustomAchivement = 0;

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
                    IsSkip = false;
                    if (DebugInput.GetKeyDown(KeyCode.Space) || SkipButton)
                    {
                        IsSkip = true;
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
                        if (IsSkip)
                        {
                            for (int i = 0; i < monitors.Length; i++)
                            {
                                if (Singleton<UserDataManager>.Instance.GetUserData(i).IsEntry)
                                {
                                    UpdateSubbMonitorDataMethod.Invoke(__instance, new object[] { i });
                                    Message[] message = (Message[]) typeof(GameProcess).GetField("_message", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
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
            catch (Exception e)
            {
                MelonLogger.Error(e);
            }
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameScoreList), "SetForceAchivement")]
        public static bool SetForceAchivement(int achivement, int dxscore, GameScoreList __instance)
        {
            decimal num1 = achivement;
            long num2;
            long num3;
            if (num1 > 100.0m)
            {
                num2 = (long)((decimal)__instance.ScoreTotal._allPerfectScore * (num1 - 1.0m) * 0.01m);
                num3 = __instance.ScoreTotal._breakBonusScore;
            }
            else
            {
                num2 = (long)((decimal)__instance.ScoreTotal._allPerfectScore * (num1 * 0.99m * 0.01m));
                num3 = (long)((decimal)__instance.ScoreTotal._breakBonusScore * num1 * 0.01m);
            }
            NoteJudge.ETiming[] NoteArray = new NoteJudge.ETiming[7]
            {
                NoteJudge.ETiming.Critical,
                NoteJudge.ETiming.FastGreat,
                NoteJudge.ETiming.FastGreat2nd,
                NoteJudge.ETiming.LateGreat,
                NoteJudge.ETiming.LateGreat2nd,
                NoteJudge.ETiming.LateGreat3rd,
                NoteJudge.ETiming.LateGood
            };
            int monitorIndex = (int) typeof(GameScoreList).GetField("_monitorIndex", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
            NoteDataList noteList = NotesManager.Instance(monitorIndex).getReader().GetNoteList();
            foreach (NoteData item in noteList)
            {
                if (!item.type.isBreakScore())
                {
                    continue;
                }
                bool flag = false;
                NoteJudge.ETiming[] array2 = NoteArray;
                foreach (NoteJudge.ETiming eTiming in array2)
                {
                    NoteScore.EScoreType eScoreType = GamePlayManager.NoteType2ScoreType(item.type.getEnum());
                    if (0m <= (decimal)(num2 - NoteScore.GetJudgeScore(eTiming, NoteScore.EScoreType.Break)) && 0m <= (decimal)(num3 - NoteScore.GetJudgeScore(eTiming, NoteScore.EScoreType.BreakBonus)))
                    {
                        num2 -= NoteScore.GetJudgeScore(eTiming, eScoreType);
                        num3 -= NoteScore.GetJudgeScore(eTiming, NoteScore.EScoreType.BreakBonus);
                        __instance.SetResult(item.indexNote, eScoreType, eTiming);
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    __instance.SetResult(item.indexNote, NoteScore.EScoreType.Break, NoteJudge.ETiming.TooFast);
                }
            }
            int num4 = 0;
            int num5 = 0;
            long num6 = 0L;
            for (int j = 0; j < NoteArray.Length; j++)
            {
                long num7 = num2;
                long num8 = 0L;
                num8 += __instance.ScoreTotal.GetTapNum() * NoteScore.GetJudgeScore(NoteArray[j]);
                num8 += __instance.ScoreTotal.GetHoldNum() * NoteScore.GetJudgeScore(NoteArray[j], NoteScore.EScoreType.Hold);
                num8 += __instance.ScoreTotal.GetSlideNum() * NoteScore.GetJudgeScore(NoteArray[j], NoteScore.EScoreType.Slide);
                num8 += __instance.ScoreTotal.GetTouchNum() * NoteScore.GetJudgeScore(NoteArray[j], NoteScore.EScoreType.Touch);
                if (num8 <= num7)
                {
                    num6 = num7 - num8;
                    num5 = ((num4 != 0) ? (num4 - 1) : 0);
                    break;
                }
                num4++;
            }
            if (num4 >= NoteArray.Length)
            {
                num4 = NoteArray.Length - 1;
            }
            foreach (NoteData item2 in noteList)
            {
                if (item2.type.isSlideScore())
                {
                    NoteScore.EScoreType eScoreType2 = GamePlayManager.NoteType2ScoreType(item2.type.getEnum());
                    NoteJudge.ETiming eTiming2 = NoteArray[num4];
                    NoteJudge.ETiming eTiming3 = NoteArray[num5];
                    if (0m <= (decimal)num6 && 0m <= (decimal)(num2 - NoteScore.GetJudgeScore(eTiming3, eScoreType2)))
                    {
                        num6 -= NoteScore.GetJudgeScore(eTiming3, eScoreType2) - NoteScore.GetJudgeScore(eTiming2, eScoreType2);
                        num2 -= NoteScore.GetJudgeScore(eTiming3, eScoreType2);
                        __instance.SetResult(item2.indexNote, eScoreType2, eTiming3);
                    }
                    else if (0m <= (decimal)(num2 - NoteScore.GetJudgeScore(eTiming2, eScoreType2)))
                    {
                        num2 -= NoteScore.GetJudgeScore(eTiming2, eScoreType2);
                        __instance.SetResult(item2.indexNote, eScoreType2, eTiming2);
                    }
                    else
                    {
                        __instance.SetResult(item2.indexNote, eScoreType2, NoteJudge.ETiming.TooFast);
                    }
                }
            }
            foreach (NoteData item3 in noteList)
            {
                if (item3.type.isHoldScore())
                {
                    NoteScore.EScoreType eScoreType3 = GamePlayManager.NoteType2ScoreType(item3.type.getEnum());
                    NoteJudge.ETiming eTiming4 = NoteArray[num4];
                    NoteJudge.ETiming eTiming5 = NoteArray[num5];
                    if (0m <= (decimal)num6 && 0m <= (decimal)(num2 - NoteScore.GetJudgeScore(eTiming5, eScoreType3)))
                    {
                        num6 -= NoteScore.GetJudgeScore(eTiming5, eScoreType3) - NoteScore.GetJudgeScore(eTiming4, eScoreType3);
                        num2 -= NoteScore.GetJudgeScore(eTiming5, eScoreType3);
                        __instance.SetResult(item3.indexNote, eScoreType3, eTiming5);
                    }
                    else if (0m <= (decimal)(num2 - NoteScore.GetJudgeScore(eTiming4, eScoreType3)))
                    {
                        num2 -= NoteScore.GetJudgeScore(eTiming4, eScoreType3);
                        __instance.SetResult(item3.indexNote, eScoreType3, eTiming4);
                    }
                    else
                    {
                        __instance.SetResult(item3.indexNote, eScoreType3, NoteJudge.ETiming.TooFast);
                    }
                }
            }
            foreach (NoteData item4 in noteList)
            {
                if (item4.type.isTapScore())
                {
                    NoteScore.EScoreType eScoreType4 = GamePlayManager.NoteType2ScoreType(item4.type.getEnum());
                    NoteJudge.ETiming eTiming6 = NoteArray[num4];
                    NoteJudge.ETiming eTiming7 = NoteArray[num5];
                    if (0m < (decimal)num6 && 0m <= (decimal)(num2 - NoteScore.GetJudgeScore(eTiming7, eScoreType4)))
                    {
                        num6 -= NoteScore.GetJudgeScore(eTiming7, eScoreType4) - NoteScore.GetJudgeScore(eTiming6, eScoreType4);
                        num2 -= NoteScore.GetJudgeScore(eTiming7, eScoreType4);
                        __instance.SetResult(item4.indexNote, eScoreType4, eTiming7);
                    }
                    else if (0m <= (decimal)(num2 - NoteScore.GetJudgeScore(eTiming6, eScoreType4)))
                    {
                        num2 -= NoteScore.GetJudgeScore(eTiming6, eScoreType4);
                        __instance.SetResult(item4.indexNote, eScoreType4, eTiming6);
                    }
                    else if (0m < (decimal)num2)
                    {
                        num2 -= NoteScore.GetJudgeScore(eTiming6, eScoreType4);
                        __instance.SetResult(item4.indexNote, eScoreType4, eTiming6);
                    }
                    else
                    {
                        __instance.SetResult(item4.indexNote, eScoreType4, NoteJudge.ETiming.TooFast);
                    }
                }
            }
            return false;
        }
    }
}
