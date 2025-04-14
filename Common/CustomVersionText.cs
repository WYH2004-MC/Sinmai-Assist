using HarmonyLib;

namespace SinmaiAssist.Common;

public class CustomVersionText
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(MAI2System.Config), "get_displayVersionString")]
    public static bool CustomVersionString(ref string __result)
    {
        if (string.IsNullOrEmpty(SinmaiAssist.MainConfig.Common.CustomVersionText.VersionText)) return true;
        __result = SinmaiAssist.MainConfig.Common.CustomVersionText.VersionText;
        return false;
    }
}