using HarmonyLib;
using Monitor;

namespace SinmaiAssist.Common;

public class ForceQuickRetry
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(QuickRetry), "IsQuickRetryEnable")]
    public static void IsQuickRetryEnable(ref bool __result)
    {
        __result = true;
    }
}