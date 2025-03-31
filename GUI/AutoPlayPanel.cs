using SinmaiAssist.Cheat;
using UnityEngine;

namespace SinmaiAssist.GUI;

public class AutoPlayPanel
{
    public static void OnGUI()
    {
        GUILayout.Label($"Mode: {AutoPlay.autoPlayMode}", MainGUI.Style.Text);
        AutoPlay.DisableUpdate = GUILayout.Toggle(AutoPlay.DisableUpdate, "Disable Mode Update");
        if (GUILayout.Button("Critical (AP+)", MainGUI.Style.Button)) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Critical;
        if (GUILayout.Button("Perfect", MainGUI.Style.Button)) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Perfect;
        if (GUILayout.Button("Great", MainGUI.Style.Button)) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Great;
        if (GUILayout.Button("Good", MainGUI.Style.Button)) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Good;
        if (GUILayout.Button("Random", MainGUI.Style.Button)) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Random;
        if (GUILayout.Button("RandomAllPerfect", MainGUI.Style.Button)) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.RandomAllPerfect;
        if (GUILayout.Button("RandomFullComboPlus", MainGUI.Style.Button)) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.RandomFullComboPlus;
        if (GUILayout.Button("RandomFullCombo", MainGUI.Style.Button)) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.RandomFullCombo;
        if (GUILayout.Button("None", MainGUI.Style.Button)) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.None;
    }
}