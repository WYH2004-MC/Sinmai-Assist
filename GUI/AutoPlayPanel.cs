using SinmaiAssist.Cheat;
using UnityEngine;

namespace SinmaiAssist.GUI;

public class AutoPlayPanel
{
    public static void OnGUI()
    {
        GUILayout.Label($"Mode: {AutoPlay.autoPlayMode}", MainGUI.Style.Text);
        AutoPlay.DisableUpdate = GUILayout.Toggle(AutoPlay.DisableUpdate, "Disable Mode Update");
        if (GUILayout.Button("Critical (AP+)", MainGUI.Style.Button) || DebugInput.GetKeyDown(SinmaiAssist.KeyBindConfig.AutoPlay.Critical.KeyCode))
            AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Critical;
        
        if (GUILayout.Button("Perfect", MainGUI.Style.Button) || DebugInput.GetKeyDown(SinmaiAssist.KeyBindConfig.AutoPlay.Perfect.KeyCode))
            AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Perfect;
        
        if (GUILayout.Button("Great", MainGUI.Style.Button) || DebugInput.GetKeyDown(SinmaiAssist.KeyBindConfig.AutoPlay.Great.KeyCode))
            AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Great;

        if (GUILayout.Button("Good", MainGUI.Style.Button) || DebugInput.GetKeyDown(SinmaiAssist.KeyBindConfig.AutoPlay.Good.KeyCode))
            AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Good;

        if (GUILayout.Button("Random", MainGUI.Style.Button) || DebugInput.GetKeyDown(SinmaiAssist.KeyBindConfig.AutoPlay.Random.KeyCode))
            AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Random;

        if (GUILayout.Button("RandomAllPerfect", MainGUI.Style.Button) || DebugInput.GetKeyDown(SinmaiAssist.KeyBindConfig.AutoPlay.RandomAllPerfect.KeyCode))
            AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.RandomAllPerfect;

        if (GUILayout.Button("RandomFullComboPlus", MainGUI.Style.Button) || DebugInput.GetKeyDown(SinmaiAssist.KeyBindConfig.AutoPlay.RandomFullComboPlus.KeyCode))
            AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.RandomFullComboPlus;

        if (GUILayout.Button("RandomFullCombo", MainGUI.Style.Button) || DebugInput.GetKeyDown(SinmaiAssist.KeyBindConfig.AutoPlay.RandomFullCombo.KeyCode))
            AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.RandomFullCombo;

        if (GUILayout.Button("None", MainGUI.Style.Button) || DebugInput.GetKeyDown(SinmaiAssist.KeyBindConfig.AutoPlay.None.KeyCode))
            AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.None;
    }
}