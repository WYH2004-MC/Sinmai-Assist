using SinmaiAssist.Common;
using UnityEngine;

namespace SinmaiAssist.GUI;

public class GraphicPanel
{
    private static string screenWidth = $"{Graphic.GetResolutionWidth()}";
    private static string screenHeight = $"{Graphic.GetResolutionHeight()}";
    private static string frameRate = $"{Graphic.GetMaxFrameRate()}";
    
    
    public static void OnGUI()
    {
        if (GUILayout.Button("Toggle full screen", GUILayout.Height(50))) Graphic.ToggleFullscreen();
        GUILayout.Label($"Custom Graphic Settings", MainGUI.Style.Title);
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.Label($"Width:");
        screenWidth = GUILayout.TextField(screenWidth);
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Label($"Height:");
        screenHeight = GUILayout.TextField(screenHeight);
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Label($"Max FPS (Unlimited is -1):");
        frameRate = GUILayout.TextField(frameRate);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Apply", GUILayout.Height(20)) && int.TryParse(screenWidth, out int widthValue) && int.TryParse(screenHeight, out int heightValue) && int.TryParse(frameRate, out int fpsValue))
        {
            if (widthValue >= 360f && heightValue >= 360f)
            {
                Graphic.SetResolution(widthValue, heightValue);
                Graphic.SetMaxFrameRate(fpsValue);
            }
        }
    }
}