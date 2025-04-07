using SinmaiAssist.Cheat;
using UnityEngine;

namespace SinmaiAssist.GUI;

public class ChartControllerPanel
{
    public static void OnGUI()
    {
        GUIStyle buttonStyle = new GUIStyle(MainGUI.Style.Button) { fontSize = 24 };
        ChartController.ButtonStatus = ChartController.Button.None;
        Manager.NotesManager notesManager = new Manager.NotesManager();
        GUILayout.Label($"Timer Status: {(notesManager.IsPlaying() ? (ChartController.IsPlaying ? "Playing" : "Paused") : "Not Play")}", MainGUI.Style.Text);
        GUILayout.Label($"Timer:", new GUIStyle(MainGUI.Style.Text) { fontSize = 20 });
        GUILayout.Label($"{ChartController.Timer.ToString("0000000.0000")}", new GUIStyle(MainGUI.Style.Title) { fontSize = 20 });
        if (GUILayout.Button($"{(ChartController.IsPlaying ? "Pause" : "Play")}", buttonStyle, GUILayout.Height(45f))) ChartController.ButtonStatus = ChartController.Button.Pause;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<<<", buttonStyle, GUILayout.Height(45f))) ChartController.ButtonStatus = ChartController.Button.TimeSkipSub3;
        if (GUILayout.Button("<<", buttonStyle, GUILayout.Height(45f))) ChartController.ButtonStatus = ChartController.Button.TimeSkipSub2;
        if (GUILayout.Button("<", buttonStyle, GUILayout.Height(45f))) ChartController.ButtonStatus = ChartController.Button.TimeSkipSub;
        if (GUILayout.Button(">", buttonStyle, GUILayout.Height(45f))) ChartController.ButtonStatus = ChartController.Button.TimeSkipAdd;
        if (GUILayout.Button(">>", buttonStyle, GUILayout.Height(45f))) ChartController.ButtonStatus = ChartController.Button.TimeSkipAdd2;
        if (GUILayout.Button(">>>", buttonStyle, GUILayout.Height(45f))) ChartController.ButtonStatus = ChartController.Button.TimeSkipAdd3;
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Reset", buttonStyle, GUILayout.Height(45f))) ChartController.ButtonStatus = ChartController.Button.Reset;
        GUILayout.Label($"RecordTime: {ChartController.RecordTime.ToString("0000000")}({-((int)ChartController.Timer - ChartController.RecordTime)})", MainGUI.Style.Text);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Set", MainGUI.Style.Button)) ChartController.ButtonStatus = ChartController.Button.Set;
        if (GUILayout.Button("Back", MainGUI.Style.Button)) ChartController.ButtonStatus = ChartController.Button.Back;
        GUILayout.EndHorizontal();
        GUILayout.Label(
            "While paused, you can use the LeftArrow and RightArrow keys to perform small range fast forward or rewind.",
            new GUIStyle(MainGUI.Style.Text)
            {
                fontSize = 11,
                wordWrap = true
            }
        );
    }
}