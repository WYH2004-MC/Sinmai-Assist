using System;
using MAI2.Util;
using Manager;
using SinmaiAssist.Utils;
using UnityEngine;

namespace SinmaiAssist.GUI;

public class DebugPanel
{
    private static readonly GUIStyle MiddleStyle = new GUIStyle()
    {
        fontSize = 12,
        normal = new GUIStyleState() { textColor = Color.white },
        alignment = TextAnchor.MiddleCenter
    };
    
    public static void OnGUI()
    {
        GUILayout.Label($"Throw Exception Test", MiddleStyle);
        if (GUILayout.Button("NullReferenceException"))
        {
            GameObject obj = null;
            obj.SetActive(true);
        }
        if (GUILayout.Button("InvalidCastException")) throw new InvalidCastException("Debug");
        GUILayout.Label($"Test Tools", MiddleStyle);
        if (GUILayout.Button("TouchArea Display")) Common.InputManager.TouchAreaDisplayButton = true;
        if (GUILayout.Button("Send Test Message"))
        {
            GameMessageManager.SendGameMessage("Hello World!\nMonitorId: 0", 0);
            GameMessageManager.SendGameMessage("Hello World!\nMonitorId: 1", 1);
        }
        if (GUILayout.Button("Save P1 Option To DefaultOption")) Common.ChangeDefaultOption.SaveOptionFile(0L);
        if (GUILayout.Button("Save P2 Option To DefaultOption")) Common.ChangeDefaultOption.SaveOptionFile(1L);
        if (GUILayout.Button("↑ ↓ ↑ ↓")) SoundManager.PlaySE(Mai2.Mai2Cue.Cue.SE_ENTRY_AIME_ERROR, 1);
        GUILayout.Label($"GUI Toggle", MiddleStyle);
        if (GUILayout.Button("Toggle Show Info")) SinmaiAssist.config.ModSetting.ShowInfo = !SinmaiAssist.config.ModSetting.ShowInfo;
        if (GUILayout.Button("Toggle Show FPS")) SinmaiAssist.config.Common.ShowFPS = !SinmaiAssist.config.Common.ShowFPS;
    }
}