using System;
using System.Runtime.CompilerServices;
using System.Text;
using MAI2.Util;
using MAI2System;
using MelonLoader;
using SinmaiAssist.Utils;
using UnityEngine;

namespace SinmaiAssist.GUI;

public class ShowVersionInfo
{

    private const int MaxConsecutiveFailures = 10;
    private static int _retryCount = 0;
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

        if (_retryCount < MaxConsecutiveFailures)
        {
            int headerLength = VersionText.Length;
            try
            {
                AppendGameInfo();
                _retryCount = 0;
            }
            catch (Exception ex)
            {
                _retryCount++;
                VersionText.Length = headerLength;
                MelonLogger.Error(ex);
                if (_retryCount < MaxConsecutiveFailures) return;
            }
        }

        if (_retryCount >= MaxConsecutiveFailures)
        {
            VersionText.AppendLine("Failed to Show version info.");
            VersionText.AppendLine("Please make sure you are running the unmodified game.");
            VersionText.AppendLine("and make sure you are using the latest version of Sinmai-Assist.");
            VersionText.AppendLine("if you think this is a bug, please report it to the developer.");
        }

        string text = VersionText.ToString();
        float height = TextStyle.CalcHeight(new GUIContent(text), 500);
        UnityEngine.GUI.Label(new Rect(10 + 2, 40 + 2, 500, height), text, TextShadow);
        UnityEngine.GUI.Label(new Rect(10, 40, 500, height), text, TextStyle);
    }

    // GetGameInfo
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void AppendGameInfo()
    {
        VersionText.AppendLine($"Client Version: {SinmaiAssist.GameID} {SinmaiAssist.GameVersion}");
        VersionText.AppendLine(
            $"Data Version: {Singleton<SystemConfig>.Instance.config.dataVersionInfo.versionNo.versionString} {Singleton<SystemConfig>.Instance.config.dataVersionInfo.versionNo.releaseNoAlphabet}");
        VersionText.AppendLine($"Current Title Server: {Server.GetTitleServerUri()}");
        var keychipId = typeof(AMDaemon.System).GetProperty("KeychipId").GetValue(null);
        VersionText.AppendLine($"Keychip: {keychipId}");
        VersionText.AppendLine($"UserId: {User.GetUserIdString(0L)} | {User.GetUserIdString(1L)}");
        if (SinmaiAssist.MainConfig.ModSetting.SafeMode)
            VersionText.AppendLine("Safe Mode");
    }
}
