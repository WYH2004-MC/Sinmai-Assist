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

        // 键盘控制部分AutoPlay模式，全大P/全小P没有意义所以不考虑增加
        if (GUILayout.Button("Great", MainGUI.Style.Button) || DebugInput.GetKeyDown(KeyCode.O))
            AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Great;

        if (GUILayout.Button("Good", MainGUI.Style.Button) || DebugInput.GetKeyDown(KeyCode.P))
            AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Good;

        if (GUILayout.Button("Random", MainGUI.Style.Button) || DebugInput.GetKeyDown(KeyCode.K))
            AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Random;

        if (GUILayout.Button("RandomAllPerfect", MainGUI.Style.Button) || DebugInput.GetKeyDown(KeyCode.G))
            AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.RandomAllPerfect;

        if (GUILayout.Button("RandomFullComboPlus", MainGUI.Style.Button) || DebugInput.GetKeyDown(KeyCode.H))
            AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.RandomFullComboPlus;

        if (GUILayout.Button("RandomFullCombo", MainGUI.Style.Button) || DebugInput.GetKeyDown(KeyCode.J))
            AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.RandomFullCombo;

        if (GUILayout.Button("None", MainGUI.Style.Button) || DebugInput.GetKeyDown(KeyCode.N))
            AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.None;
    }
}