using HarmonyLib;
using MAI2System;

namespace SinmaiAssist.Fix;

public class DisableIniClear
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(IniFile), "Clear")]
    public static bool DisableClearMethod(IniFile __instance)
    {
        return false;
    }
}