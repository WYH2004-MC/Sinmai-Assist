using HarmonyLib;
using Manager;
using MelonLoader;
using Monitor.ModeSelect;
using SinmaiAssist.Utils;

namespace SinmaiAssist.Common;

public class AutoBackupData
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ModeSelectMonitor), "Initialize")]
    public static void Initialize(ModeSelectMonitor __instance, int monIndex)
    { 
        UserData userData = User.GetUserData(monIndex);
        if (userData.IsGuest()) return;
        string fileName = User.ExportBackupData(monIndex, sendMessage:false);
        MelonLogger.Msg($"[AutoBackupData] P{monIndex + 1} Player {userData.Detail.UserName}({userData.Detail.UserID}) Backup Success: {fileName} ");
    }
}