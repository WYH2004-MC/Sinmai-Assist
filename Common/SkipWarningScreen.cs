using HarmonyLib;
using Monitor;

namespace SinmaiAssist.Common;

public class SkipWarningScreen
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(WarningMonitor), "PlayLogo")]
    public static bool PlayLogo()
    {
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WarningMonitor), "IsLogoAnimationEnd")]
    public static bool IsLogoAnimationEnd(ref bool __result)
    {
        __result = true;
        return false;
    }
}