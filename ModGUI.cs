using Cheat;
using MAI2.Util;
using MAI2System;
using System;
using UnityEngine;

namespace SinmaiAssist
{
    public class ModGUI
    {
        private enum Toolbar
        {
            AutoPlay,
            FastSkip,
            DummyLogin,

            Debug
        }
        public static string DummyQrCode;
        public static string DummyUserId = "0";
        public static bool QrLoginFlag = false;
        public static bool UserIdLoginFlag = false;

        private Rect PanelWindow;
        private string VersionText;
        private GUIStyle TextStyle;
        private GUIStyle TextShadowStyle;
        private GUIStyle BigTextStyle;
        private Single PanelWidth;
        private int buttonsPerRow;
        private Toolbar toolbar = Toolbar.AutoPlay;
        private string[] ToolbarStrings = new string[] { "AutoPlay", "FastSkip", "DummyLogin" };

        public ModGUI()
        {
            PanelWindow = new Rect();
            TextStyle = new GUIStyle();
            TextShadowStyle = new GUIStyle();
            BigTextStyle = new GUIStyle();
            TextStyle.fontSize = 24;
            TextStyle.normal.textColor = Color.white;
            TextStyle.alignment = TextAnchor.UpperLeft;
            TextShadowStyle.fontSize = 24;
            TextShadowStyle.normal.textColor = Color.black;
            TextShadowStyle.alignment = TextAnchor.UpperLeft;
            BigTextStyle.fontSize = 40;
            BigTextStyle.alignment = TextAnchor.MiddleCenter;
            BigTextStyle.normal.textColor = Color.white;
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
            else if (toolbar == Toolbar.FastSkip) FastSkipPanel();
            else if (toolbar == Toolbar.DummyLogin && SinmaiAssist.config.DummyLogin && SinmaiAssist.IsSDGB) DummyLoginPanel();
            else DisablePanel();
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private void AutoPlayPanel()
        {
            GUILayout.Label($"IsAutoPlay: {AutoPlay.IsAutoPlay()}");
            GUILayout.Label($"Mode: {AutoPlay.autoPlayMode}");
            if (GUILayout.Button("Critical")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Critical;
            if (GUILayout.Button("Perfect")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Perfect;
            if (GUILayout.Button("Great")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Great;
            if (GUILayout.Button("Good")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Good;
            if (GUILayout.Button("Random")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.Random;
            if (GUILayout.Button("RandomAllPerfect")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.RandomAllPerfect;
            if (GUILayout.Button("RandomFullComblePlus")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.RandomFullComblePlus;
            if (GUILayout.Button("RandomFullComble")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.RandomFullComble;
            if (GUILayout.Button("None")) AutoPlay.autoPlayMode = AutoPlay.AutoPlayMode.None;
        }

        private void FastSkipPanel()
        {
            GUILayout.Label("施工中.", BigTextStyle, GUILayout.Width(PanelWidth), GUILayout.Height(280f));
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
            VersionText = (
                $"{BuildInfo.Name} {BuildInfo.Version}\n" +
                $"Powered by MelonLoader\n" +
                $"Client Version: {SinmaiAssist.gameID} {SinmaiAssist.gameVersion}\n" +
                $"Data Version: {Singleton<SystemConfig>.Instance.config.dataVersionInfo.versionNo.versionString} {Singleton<SystemConfig>.Instance.config.dataVersionInfo.versionNo.releaseNoAlphabet}\n" +
                $"Keychip: {AMDaemon.System.KeychipId}"
                );
            if (SinmaiAssist.config.SafeMode)
                VersionText += "\nSafe Mode";
            GUI.Label(new Rect(10+2, 40+2, 500, 30), VersionText, TextShadowStyle);
            GUI.Label(new Rect(10, 40, 500, 30), VersionText, TextStyle);
        }
    }
}
