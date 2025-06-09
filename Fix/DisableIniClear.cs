using HarmonyLib;
using MAI2System;

namespace SinmaiAssist.Fix;

public class DisableIniClear
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(IniFile), "clear")]
    public static bool DisableClearMethod(IniFile __instance)
    {
        return false;
    }
}