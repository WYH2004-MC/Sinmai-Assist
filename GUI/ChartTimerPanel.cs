using SinmaiAssist.Cheat;
using UnityEngine;

namespace SinmaiAssist.GUI;

public class ChartTimerPanel
{
    public static void OnGUI()
    {
        GUIStyle buttonStyle = new GUIStyle(UnityEngine.GUI.skin.button) { fontSize = 24 };
        ChartTimer.ButtonStatus = ChartTimer.Button.None;
        Manager.NotesManager notesManager = new Manager.NotesManager();
        GUILayout.Label($"Timer Status: {(notesManager.IsPlaying() ? (ChartTimer.IsPlaying ? "Playing" : "Paused") : "Not Play")}", MainGUI.Style.Text);
        GUILayout.Label($"Timer:", new GUIStyle(MainGUI.Style.Text) { fontSize = 20 });
        GUILayout.Label($"{ChartTimer.Timer.ToString("0000000.0000")}", new GUIStyle(MainGUI.Style.Title) { fontSize = 20 });
        if (GUILayout.Button($"{(ChartTimer.IsPlaying ? "Pause" : "Play")}", buttonStyle, GUILayout.Height(45f))) ChartTimer.ButtonStatus = ChartTimer.Button.Pause;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<<<", buttonStyle, GUILayout.Height(45f))) ChartTimer.ButtonStatus = ChartTimer.Button.TimeSkipSub3;
        if (GUILayout.Button("<<", buttonStyle, GUILayout.Height(45f))) ChartTimer.ButtonStatus = ChartTimer.Button.TimeSkipSub2;
        if (GUILayout.Button("<", buttonStyle, GUILayout.Height(45f))) ChartTimer.ButtonStatus = ChartTimer.Button.TimeSkipSub;
        if (GUILayout.Button(">", buttonStyle, GUILayout.Height(45f))) ChartTimer.ButtonStatus = ChartTimer.Button.TimeSkipAdd;
        if (GUILayout.Button(">>", buttonStyle, GUILayout.Height(45f))) ChartTimer.ButtonStatus = ChartTimer.Button.TimeSkipAdd2;
        if (GUILayout.Button(">>>", buttonStyle, GUILayout.Height(45f))) ChartTimer.ButtonStatus = ChartTimer.Button.TimeSkipAdd3;
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Reset", buttonStyle, GUILayout.Height(45f))) ChartTimer.ButtonStatus = ChartTimer.Button.Reset;
        GUILayout.Label($"RecordTime: {ChartTimer.recordTime.ToString("0000000")}({-((int)ChartTimer.Timer - ChartTimer.recordTime)})", MainGUI.Style.Text);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Set")) ChartTimer.ButtonStatus = ChartTimer.Button.Set;
        if (GUILayout.Button("Back")) ChartTimer.ButtonStatus = ChartTimer.Button.Back;
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