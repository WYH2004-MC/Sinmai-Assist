using System;
using System.Reflection;
using AMDaemon;
using HarmonyLib;
using Manager;
using SinmaiAssist;

namespace SinmaiAssist.Common;

public class DummyAimeLogin
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(AimeUnit), "HasConfirm")]
    public static bool HasConfirm(ref bool __result)
    {
        if (ModGUI.UserIdLoginFlag)
        {
            __result = ModGUI.UserIdLoginFlag;
            return false;
        }
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AimeUnit), "HasResult")]
    public static bool HasResult(ref bool __result)
    {
        if (ModGUI.UserIdLoginFlag)
        {
            __result = ModGUI.UserIdLoginFlag;
            return false;
        }
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AimeUnit), "HasError")]
    public static bool HasError(ref bool __result)
    {
        if (ModGUI.UserIdLoginFlag)
        {
            __result = !ModGUI.UserIdLoginFlag;
            return false;
        }
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AimeUnit), "ErrorInfo")]
    public static bool GetResult(ref AimeErrorId __result)
    {
        if (ModGUI.UserIdLoginFlag)
        {
            __result = AimeErrorId.None;
            return false;
        }
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AimeResult), "IsValid")]
    public static bool IsValid(ref bool __result)
    {
        if (ModGUI.UserIdLoginFlag)
        {
            __result = true;
            return false;
        }
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AimeResult), "AccessCode")]
    public static bool GetAccessCode(ref AccessCode __result)
    {
        if (ModGUI.UserIdLoginFlag)
        {
            Random random = new Random();
            __result = AccessCode.Make((random.NextDouble() * (long)Math.Pow(10, 20)).ToString());
            return false;
        }
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AimeResult), "AimeId")]
    public static bool GetAimeId(ref AimeId __result)
    {
        if (ModGUI.UserIdLoginFlag)
        {
            __result = new AimeId(Convert.ToUInt32(ModGUI.DummyUserId));
            return false;
        }
        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Process.Entry.TryAime), "Execute")]
    public static void ClearFlag()
    {
        ModGUI.UserIdLoginFlag = false;
    }
}