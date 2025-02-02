using HarmonyLib;

namespace SinmaiAssist.Common;

public class InfinityTimerLegacy
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimerController), "PrepareTimer")]
    public static void PrePrepareTimer(ref int second)
    {
        second = 65535;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(GenericMonitor), "SetTimerSecond")]
    public static bool ForceInfinity(ref bool isInfinity)
    {
        isInfinity = true;
        return true;
    }
}