using SinmaiAssist.Cheat;
using UnityEngine;

namespace SinmaiAssist.GUI;

public class AutoPlayPanel
{
    public static void OnGUI()
    {
        GUILayout.Label($"Mode: {AutoPlay.autoPlayMode}");
        AutoPlay.DisableUpdate = GUILayout.Toggle(AutoPlay.DisableUpdate, "Disable Mode Update");
        if (GUILayout.Button("Critical (AP+)")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Critical;
        if (GUILayout.Button("Perfect")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Perfect;
        if (GUILayout.Button("Great")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Great;
        if (GUILayout.Button("Good")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Good;
        if (GUILayout.Button("Random")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Random;
        if (GUILayout.Button("RandomAllPerfect")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.RandomAllPerfect;
        if (GUILayout.Button("RandomFullComboPlus")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.RandomFullComboPlus;
        if (GUILayout.Button("RandomFullCombo")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.RandomFullCombo;
        if (GUILayout.Button("None")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.None;
    }
}