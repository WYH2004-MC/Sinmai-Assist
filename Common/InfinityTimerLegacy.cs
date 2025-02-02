using HarmonyLib;

namespace SinmaiAssist.Common;

public class InfinityTimerLegacy
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimerController), "UpdateTimer")]
    public static bool UpdateTimer(ref float gameMsecAdd)
    {
        gameMsecAdd = 0;
        return true;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(GenericMonitor), "SetTimerSecond")]
    public static bool ForceInfinity(ref bool isInfinity)
    {
        isInfinity = true;
        return true;
    }
}