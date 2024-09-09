using Cheat;
using MAI2.Util;
using MAI2System;
using Manager;
using System;
using System.Text;
using UnityEngine;
using Utils;

namespace SinmaiAssist
{
    public class ModGUI
    {
        private enum Toolbar
        {
            AutoPlay,
            FastSkip,
            ChartTimer,
            DummyLogin,
            Debug
        }
        public static string DummyQrCode;
        public static string DummyUserId = "0";
        public static string achivementInput = "0";
        public static bool QrLoginFlag = false;
        public static bool UserIdLoginFlag = false;

        private Rect PanelWindow;
        private StringBuilder VersionText = new StringBuilder();
        private GUIStyle MiddleStyle;
        private GUIStyle TextStyle;
        private GUIStyle TextShadowStyle;
        private GUIStyle BigTextStyle;
        private GUIStyle errorStyle;
        private Single PanelWidth;
        private int buttonsPerRow;
        private Toolbar toolbar = Toolbar.AutoPlay;
        private string[] ToolbarStrings = new string[] { "AutoPlay", "FastSkip", "DummyLogin" };

        public ModGUI()
        {
            PanelWindow = new Rect();
            MiddleStyle = new GUIStyle();
            TextStyle = new GUIStyle();
            TextShadowStyle = new GUIStyle();
            BigTextStyle = new GUIStyle();
            errorStyle = new GUIStyle();
            MiddleStyle.fontSize = 12;
            MiddleStyle.normal.textColor = Color.white;
            MiddleStyle.alignment = TextAnchor.MiddleCenter;
            TextStyle.fontSize = 24;
            TextStyle.normal.textColor = Color.white;
            TextStyle.alignment = TextAnchor.UpperLeft;
            TextShadowStyle.fontSize = 24;
            TextShadowStyle.normal.textColor = Color.black;
            TextShadowStyle.alignment = TextAnchor.UpperLeft;
            BigTextStyle.fontSize = 40;
            BigTextStyle.alignment = TextAnchor.MiddleCenter;
            BigTextStyle.normal.textColor = Color.white;
            errorStyle.normal.textColor = Color.red;
            PanelWidth = 300f;
            buttonsPerRow = 3;
        }

        public void OnGUI()
        {
            PanelWindow = GUILayout.Window(10086, PanelWindow, MainPanel, $"{BuildInfo.Name} {BuildInfo.Version}");
            if (SinmaiAssist.config.ShowVersionInfo) ShowVersionInfo();
        }

        private void MainPanel(int id)
        {
            GUILayout.BeginVertical($"{BuildInfo.Name} {BuildInfo.Version}", GUILayout.Height(20f));
            ToolBarPanel();
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUILayout.Width(PanelWidth), GUILayout.Height(280f));
            if (toolbar == Toolbar.AutoPlay && SinmaiAssist.config.AutoPlay) AutoPlayPanel();
            else if (toolbar == Toolbar.FastSkip && SinmaiAssist.config.FastSkip) FastSkipPanel();
            else if (toolbar == Toolbar.ChartTimer && SinmaiAssist.config.ChartTimer) ChartTimerPanel();
            else if (toolbar == Toolbar.DummyLogin && SinmaiAssist.config.DummyLogin && SinmaiAssist.IsSDGB) DummyLoginPanel();
            else if (toolbar == Toolbar.Debug) DebugPanel();
            else DisablePanel();
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private void AutoPlayPanel()
        {
            GUILayout.Label($"IsAutoPlay: {AutoPlay.IsAutoPlay()}");
            GUILayout.Label($"Mode: {AutoPlay.autoPlayMode}");
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

        private void FastSkipPanel()
        {
            FastSkip.SkipButton = false;
            GUILayout.Label($"Skip Mode: {(FastSkip.CustomSkip ? "Custom" : "Default")}");
            if (GUILayout.Button("Skip", new GUIStyle(GUI.skin.button){ fontSize=20 }, GUILayout.Height(45f))) FastSkip.SkipButton = true;
            GUILayout.Label($"Mode Setting", MiddleStyle);
            if (GUILayout.Button("Default")) FastSkip.CustomSkip = false;
            if (GUILayout.Button("Custom")) FastSkip.CustomSkip = true;
            if (FastSkip.CustomSkip)
            {
                GUILayout.Label($"Custom Setting", MiddleStyle);
                GUILayout.Label($"Custom Achivement(0 - 101): ");
                achivementInput = GUILayout.TextField(achivementInput);
                if (int.TryParse(achivementInput, out int achivementValue))
                {
                    if (achivementValue >= 0f && achivementValue <= 101f)
                    {
                        FastSkip.CustomAchivement = achivementValue;
                        GUILayout.Label($"Custom Achivement: {achivementValue} %");
                    }
                    else
                    {
                        GUILayout.Label("Error: Please enter a value between 0 and 101.", errorStyle);
                    }
                }
                else
                {
                    GUILayout.Label("Error: Please enter a valid int value.", errorStyle);
                }
            }
        }

        private void ChartTimerPanel()
        {
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 24 };
            ChartTimer.ButtonStatus = ChartTimer.Button.None;
            Manager.NotesManager notesManager = new Manager.NotesManager();
            GUILayout.Label($"Timer Status: {(notesManager.IsPlaying() ? (ChartTimer.IsPlaying ? "Playing" : "Paused") : "Not Play")}");
            GUILayout.Label($"Timer:", new GUIStyle(GUI.skin.label) { fontSize = 20 });
            GUILayout.Label($"{ChartTimer.Timer.ToString("0000000.0000")}", new GUIStyle(MiddleStyle) { fontSize = 20 });
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
            GUILayout.Label(
                "While paused, you can use the LeftArrow and RightArrow keys to perform small range fast forward or rewind.",
                new GUIStyle(GUI.skin.label)
                {
                    fontSize = 12,
                    wordWrap = true
                }
                );
        }

        private void DummyLoginPanel()
        {
            GUILayout.Label("QrCode:");
            DummyQrCode = GUILayout.TextArea(DummyQrCode, GUILayout.Height(100f));
            if (GUILayout.Button("QrCode Login")) QrLoginFlag = true;
            GUILayout.Space(10f);
            GUILayout.Label("UserID:");
            DummyUserId = GUILayout.TextField(DummyUserId, GUILayout.Height(20f));
            if (GUILayout.Button("UserId Login")) UserIdLoginFlag = true;
        }

        private void DebugPanel()
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
        }

        private void DisablePanel()
        {
            GUILayout.Label(toolbar.ToString(), BigTextStyle, GUILayout.Width(PanelWidth), GUILayout.Height(120f));
            GUILayout.Label("is Disable", BigTextStyle, GUILayout.Width(PanelWidth), GUILayout.Height(160f));
        }

        private void ToolBarPanel()
        {
            string[] toolbarNames = Enum.GetNames(typeof(Toolbar));
            int toolbarCount = toolbarNames.Length;
            int rowCount = Mathf.CeilToInt((float)toolbarCount / buttonsPerRow);
            int selectedToolbar = (int)toolbar;

            for (int row = 0; row < rowCount; row++)
            {
                GUILayout.BeginHorizontal();

                int startIndex = row * buttonsPerRow;
                int endIndex = Mathf.Min(startIndex + buttonsPerRow, toolbarCount);

                string[] rowToolbarNames = new string[endIndex - startIndex];
                Array.Copy(toolbarNames, startIndex, rowToolbarNames, 0, endIndex - startIndex);

                int rowSelection = GUILayout.Toolbar(selectedToolbar - startIndex, rowToolbarNames, GUILayout.Width(PanelWidth), GUILayout.Height(20f));
                selectedToolbar = startIndex + rowSelection;

                GUILayout.EndHorizontal();
            }
            toolbar = (Toolbar)selectedToolbar;
        }

        private void ShowVersionInfo()
        {
            VersionText.Clear();
            VersionText.AppendLine($"{BuildInfo.Name} {BuildInfo.Version}");
            VersionText.AppendLine("Powered by MelonLoader");
            VersionText.AppendLine($"Client Version: {SinmaiAssist.gameID} {SinmaiAssist.gameVersion}");
            VersionText.AppendLine(
                $"Data Version: {Singleton<SystemConfig>.Instance.config.dataVersionInfo.versionNo.versionString} {Singleton<SystemConfig>.Instance.config.dataVersionInfo.versionNo.releaseNoAlphabet}");
            VersionText.AppendLine($"Current Title Server: {Server.GetTitleServerUri()}");
            VersionText.AppendLine($"Keychip: {AMDaemon.System.KeychipId}");
            VersionText.AppendLine($"UserId: {Singleton<UserDataManager>.Instance.GetUserData(0L).Detail.UserID} | {Singleton<UserDataManager>.Instance.GetUserData(1L).Detail.UserID}");
            if (SinmaiAssist.config.SafeMode)
                VersionText.AppendLine("Safe Mode");
            GUI.Label(new Rect(10+2, 40+2, 500, 30), VersionText.ToString(), TextShadowStyle);
            GUI.Label(new Rect(10, 40, 500, 30), VersionText.ToString(), TextStyle);
        }
    }
}
