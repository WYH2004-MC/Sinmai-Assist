using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.RegularExpressions;
using AMDaemon;
using HarmonyLib;
using Mai2.Mai2Cue;
using Manager;
using SinmaiAssist.Utils;

namespace SinmaiAssist.Common;

public class DummyAimeLogin
{
    public static void ReadCard(string accesscode=null, string oldCode=null)
    {
        accesscode ??= ModGUI.DummyQrCode.Trim();
        if (!Directory.Exists("DEVICE")) Directory.CreateDirectory("DEVICE");
        if (accesscode.Length == 20 && Regex.IsMatch(accesscode, @"^\d+$"))
        {
            File.WriteAllText("DEVICE/aime.txt", accesscode);
            Keyboard.LongPressKey(Keys.Enter, 300);
            if (oldCode is { Length: 20 } && Regex.IsMatch(oldCode, @"^\d+$")) 
                File.WriteAllText("DEVICE/aime.txt", oldCode);
        }
        else
        {
            GameMessageManager.SendGameMessage("<color=\"red\">Failed to read Aime!");
            SoundManager.PlaySE(Cue.SE_ENTRY_AIME_ERROR, 1);
        }
    }
    //
    // [HarmonyPrefix]
    // [HarmonyPatch(typeof(AimeUnit), "HasConfirm")]
    // public static bool HasConfirm(ref bool __result)
    // {
    //     if (ModGUI.UserIdLoginFlag)
    //     {
    //         __result = ModGUI.UserIdLoginFlag;
    //         return false;
    //     }
    //     return true;
    // }
    //
    // [HarmonyPrefix]
    // [HarmonyPatch(typeof(AimeUnit), "HasResult")]
    // public static bool HasResult(ref bool __result)
    // {
    //     if (ModGUI.UserIdLoginFlag)
    //     {
    //         __result = ModGUI.UserIdLoginFlag;
    //         return false;
    //     }
    //     return true;
    // }
    //
    // [HarmonyPrefix]
    // [HarmonyPatch(typeof(AimeUnit), "HasError")]
    // public static bool HasError(ref bool __result)
    // {
    //     if (ModGUI.UserIdLoginFlag)
    //     {
    //         __result = !ModGUI.UserIdLoginFlag;
    //         return false;
    //     }
    //     return true;
    // }
    //
    // [HarmonyPrefix]
    // [HarmonyPatch(typeof(AimeUnit), "ErrorInfo")]
    // public static bool GetResult(ref AimeErrorId __result)
    // {
    //     if (ModGUI.UserIdLoginFlag)
    //     {
    //         __result = AimeErrorId.None;
    //         return false;
    //     }
    //     return true;
    // }
    //
    // [HarmonyPrefix]
    // [HarmonyPatch(typeof(AimeResult), "IsValid")]
    // public static bool IsValid(ref bool __result)
    // {
    //     if (ModGUI.UserIdLoginFlag)
    //     {
    //         __result = true;
    //         return false;
    //     }
    //     return true;
    // }

    // [HarmonyPrefix]
    // [HarmonyPatch(typeof(AimeResult), "AccessCode")]
    // public static bool GetAccessCode(ref AccessCode __result)
    // {
    //     if (ModGUI.UserIdLoginFlag)
    //     {
    //         Random random = new Random();
    //         __result = AccessCode.Make((random.NextDouble() * (long)Math.Pow(10, 20)).ToString());
    //         return false;
    //     }
    //     return true;
    // }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AimeId), "Value", MethodType.Getter)]
    public static bool GetAimeId(ref uint __result)
    {
        if (ModGUI.UserIdLoginFlag)
        {
            __result = Convert.ToUInt32(ModGUI.DummyUserId);
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