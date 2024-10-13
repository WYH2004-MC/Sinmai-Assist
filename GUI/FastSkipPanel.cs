using SinmaiAssist.Cheat;
using UnityEngine;

namespace SinmaiAssist.GUI;

public class FastSkipPanel
{
    private static string _scoreInput = "0";
    
    public static void OnGUI()
    {
        FastSkip.SkipButton = false;
        GUILayout.Label($"Skip Mode: {(FastSkip.CustomSkip ? "Custom" : "Default")}", MainGUI.Style.Text);
        if (GUILayout.Button("Skip", new GUIStyle(UnityEngine.GUI.skin.button){ fontSize=20 }, GUILayout.Height(45f))) FastSkip.SkipButton = true;
        GUILayout.Label($"Mode Setting", MainGUI.Style.Title);
        if (GUILayout.Button("Default")) FastSkip.CustomSkip = false;
        if (GUILayout.Button("Custom")) FastSkip.CustomSkip = true;
        if (FastSkip.CustomSkip)
        {
            GUILayout.Label($"Custom Setting", MainGUI.Style.Title);
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Custom Score(0 - 101): ", MainGUI.Style.Text);
            _scoreInput = GUILayout.TextField(_scoreInput);
            GUILayout.EndHorizontal();
            if (int.TryParse(_scoreInput, out int scoreValue))
            {
                if (scoreValue >= 0f && scoreValue <= 101f)
                {
                    FastSkip.CustomAchivement = scoreValue;
                    GUILayout.Label($"Custom Score: {scoreValue} %", MainGUI.Style.Text);
                }
                else
                {
                    GUILayout.Label("Error: Please enter a value between 0 and 101.", MainGUI.Style.ErrorMessage);
                }
            }
            else
            {
                GUILayout.Label("Error: Please enter a valid int value.", MainGUI.Style.ErrorMessage);
            }
            FastSkip.Force1Miss = GUILayout.Toggle(FastSkip.Force1Miss, "Force Add 1 miss");
        }
    }
}