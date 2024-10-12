using System.Text;
using MAI2.Util;
using MAI2System;
using SinmaiAssist.Utils;
using UnityEngine;

namespace SinmaiAssist.GUI;

public class ShowVersionInfo
{
    private static readonly StringBuilder VersionText = new StringBuilder();

    private static readonly GUIStyle TextShadow = new GUIStyle()
    {
        fontSize = 24,
        alignment = TextAnchor.UpperLeft,
        normal = new GUIStyleState(){textColor = Color.black}
    };

    private static readonly GUIStyle TextStyle = new GUIStyle()
    {
        fontSize = 24,
        alignment = TextAnchor.UpperLeft,
        normal = new GUIStyleState(){textColor = Color.white}
    };
    
    public static void OnGUI()
    {
        VersionText.Clear();
        VersionText.AppendLine($"{BuildInfo.Name} {BuildInfo.Version} ({BuildInfo.CommitHash})");
        VersionText.AppendLine("Powered by MelonLoader");
        VersionText.AppendLine($"Client Version: {SinmaiAssist.gameID} {SinmaiAssist.gameVersion}");
        VersionText.AppendLine(
            $"Data Version: {Singleton<SystemConfig>.Instance.config.dataVersionInfo.versionNo.versionString} {Singleton<SystemConfig>.Instance.config.dataVersionInfo.versionNo.releaseNoAlphabet}");
        VersionText.AppendLine($"Current Title Server: {Server.GetTitleServerUri()}");
        VersionText.AppendLine($"Keychip: {AMDaemon.System.KeychipId}");
        VersionText.AppendLine($"UserId: {User.GetUserIdString(0L)} | {User.GetUserIdString(1L)}");
        if (SinmaiAssist.config.ModSetting.SafeMode)
            VersionText.AppendLine("Safe Mode");
        UnityEngine.GUI.Label(new Rect(10+2, 40+2, 500, 30), VersionText.ToString(), TextShadow);
        UnityEngine.GUI.Label(new Rect(10, 40, 500, 30), VersionText.ToString(), TextStyle);
    }
}