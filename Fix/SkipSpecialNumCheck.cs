using System;
using HarmonyLib;
using Manager;
using MelonLoader;

namespace SinmaiAssist.Fix;

public class SkipSpecialNumCheck
{
    // codes from AquaMai [https://github.com/MewoLab/AquaMai/blob/main/AquaMai.Mods/Fix/Common.cs]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameManager), "CalcSpecialNum")]
    private static void CalcSpecialNum(ref int __result)
    {
#if DEBUG
        MelonLogger.Warning($"[SkipSpecialNumCheck] OriginalSpecialNum:{__result.ToString()}, {Convert.ToString(__result, 2)}");
#endif
        var num = 0;
        num |= 1024;
        __result = num;
    }
}