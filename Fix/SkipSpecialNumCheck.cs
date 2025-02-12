using HarmonyLib;
using Manager;

namespace SinmaiAssist.Fix;

public class SkipSpecialNumCheck
{
    // codes from AquaMai [https://github.com/MewoLab/AquaMai/blob/main/AquaMai.Mods/Fix/Common.cs]
    public static void OnAfterPatch(HarmonyLib.Harmony h)
    {
        if (typeof(GameManager).GetMethod("CalcSpecialNum") is null) return;
        h.PatchAll(typeof(CalcSpecialNumPatch));
    }

    private class CalcSpecialNumPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameManager), "CalcSpecialNum")]
        private static bool CalcSpecialNum(ref int __result)
        {
            __result = 1024;
            return false;
        }
    }
}