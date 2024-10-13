using System;
using MAI2.Util;
using Manager;
using SinmaiAssist.Utils;
using UnityEngine;

namespace SinmaiAssist.GUI;

public class DebugPanel
{
    public static void OnGUI()
    {
        GUILayout.Label($"Throw Exception Test", MainGUI.Style.Title);
        if (GUILayout.Button("NullReferenceException"))
        {
            GameObject obj = null;
            obj.SetActive(true);
        }
        if (GUILayout.Button("InvalidCastException")) throw new InvalidCastException("Debug");
        GUILayout.Label($"Test Tools", MainGUI.Style.Title);
        if (GUILayout.Button("TouchArea Display")) Common.InputManager.TouchAreaDisplayButton = true;
        if (GUILayout.Button("Send Test Message"))
        {
            GameMessageManager.SendMessage(0,"Hello World!\nMonitorId: 0");
            GameMessageManager.SendMessage(1,"Hello World!\nMonitorId: 1");
        }
        if (GUILayout.Button("Save P1 Option To DefaultOption")) Common.ChangeDefaultOption.SaveOptionFile(0L);
        if (GUILayout.Button("Save P2 Option To DefaultOption")) Common.ChangeDefaultOption.SaveOptionFile(1L);
        if (GUILayout.Button("↑ ↓ ↑ ↓")) SoundManager.PlaySE(Mai2.Mai2Cue.Cue.SE_ENTRY_AIME_ERROR, 1);
        GUILayout.Label($"GUI Toggle", MainGUI.Style.Title);
        if (GUILayout.Button("Toggle Show Info")) SinmaiAssist.config.ModSetting.ShowInfo = !SinmaiAssist.config.ModSetting.ShowInfo;
        if (GUILayout.Button("Toggle Show FPS")) SinmaiAssist.config.Common.ShowFPS = !SinmaiAssist.config.Common.ShowFPS;
    }
}