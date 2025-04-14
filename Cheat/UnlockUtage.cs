using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using MAI2System;
using Manager;
using Manager.MaiStudio;
using MelonLoader;
using SinmaiAssist.Utils;

namespace SinmaiAssist.Cheat;

public class UnlockUtage
{
    private static List<int> _needDoublePlayerMusicIdList = [];
    
    private static ForceUtageModeType _forceUtageMode = ForceUtageModeType.Normal;

    private static Dictionary<ForceUtageModeType, string> _utagePathMap = new()
    {
        { ForceUtageModeType.Normal, "" },
        { ForceUtageModeType.Force1P, "_L.ma2" },
        { ForceUtageModeType.Force2P, "_R.ma2" }
    };
    
    private enum ForceUtageModeType
    {
        Normal,
        Force1P,
        Force2P
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameManager), "CanUnlockUtageTotalJudgement")]
    public static bool ForceUnlockUtage(out ConstParameter.ResultOfUnlockUtageJudgement result1P, out ConstParameter.ResultOfUnlockUtageJudgement result2P)
    {
        result1P = ConstParameter.ResultOfUnlockUtageJudgement.Unlocked;
        result2P = ConstParameter.ResultOfUnlockUtageJudgement.Unlocked;
        return false;
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameScoreList), "InitalizeUtage")]
    public static void InitalizeUtage(GameScoreList __instance, int monitorIndex)
    {
        if (_needDoublePlayerMusicIdList.Contains(__instance.SessionInfo.musicId))
        {
            if (_forceUtageMode == 0)
            {
                if (monitorIndex == 0) __instance.SessionInfo.dPScoreFilePathAdd = "_L.ma2";
                if (monitorIndex == 1) __instance.SessionInfo.dPScoreFilePathAdd = "_R.ma2";
            }
            else
            {
                __instance.SessionInfo.dPScoreFilePathAdd = _utagePathMap[_forceUtageMode];
            }
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MusicData), "Init")]
    public static void ForceSetUtagePlayStyle(MusicData __instance)
    {
        if(!SinmaiAssist.MainConfig.Cheat.UnlockUtage.UnlockDoublePlayerMusic) return;
        if (__instance.utagePlayStyle == UtagePlayStyle.DoublePlayerScore)
        {
            PropertyInfo utagePlayStyleProperty = typeof(MusicData).GetProperty("utagePlayStyle", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            utagePlayStyleProperty.SetValue(__instance, UtagePlayStyle.SinglePlayerScore);
            _needDoublePlayerMusicIdList.Add(__instance.GetID());
            #if Debug
            MelonLogger.Msg($"DoublePlayerScoreLoadEvent: {__instance.GetID()}");
            #endif
        }
    }
    
    public static void SwitchUtageMode()
    {
        int current = (int)_forceUtageMode;
        int next = (current + 1) % 3;
        _forceUtageMode = (ForceUtageModeType)next;
        GameMessageManager.SendMessage(0, $"Force Utage Mode: \n{_forceUtageMode.ToString()}");
        GameMessageManager.SendMessage(1, $"Force Utage Mode: \n{_forceUtageMode.ToString()}");
    }
}