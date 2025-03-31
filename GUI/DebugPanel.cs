using System;
using MAI2.Util;
using Manager;
using SinmaiAssist.Utils;
using UnityEngine;

namespace SinmaiAssist.GUI;

public class DebugPanel
{
    public static bool UnityLogger = false;
    public static void OnGUI()
    {
        GUILayout.Label($"Test Tools", MainGUI.Style.Title);
        if (GUILayout.Button("TouchArea Display", MainGUI.Style.Button)) Common.InputManager.TouchAreaDisplayButton = true;
        if (GUILayout.Button("Send Test Message", MainGUI.Style.Button))
        {
            GameMessageManager.SendMessage(0,"Hello World!\nMonitorId: 0");
            GameMessageManager.SendMessage(1,"Hello World!\nMonitorId: 1");
        }
        if (GUILayout.Button("Save P1 Option To DefaultOption", MainGUI.Style.Button)) Common.ChangeDefaultOption.SaveOptionFile(0L);
        if (GUILayout.Button("Save P2 Option To DefaultOption", MainGUI.Style.Button)) Common.ChangeDefaultOption.SaveOptionFile(1L);
        if (GUILayout.Button("↑ ↓ ↑ ↓", MainGUI.Style.Button)) SoundManager.PlaySE(Mai2.Mai2Cue.Cue.SE_ENTRY_AIME_ERROR, 1);
        GUILayout.Label($"GUI Toggle", MainGUI.Style.Title);
        if (GUILayout.Button("Toggle Show Info", MainGUI.Style.Button)) SinmaiAssist.config.ModSetting.ShowInfo = !SinmaiAssist.config.ModSetting.ShowInfo;
        if (GUILayout.Button("Toggle Show FPS", MainGUI.Style.Button)) SinmaiAssist.config.Common.ShowFPS = !SinmaiAssist.config.Common.ShowFPS;
        UnityLogger = GUILayout.Toggle(UnityLogger, "Log Unity Debug?");
    }
}