using Cheat;
using MAI2.Util;
using MAI2System;
using UnityEngine;

namespace SinmaiAssist
{
    public class ModGUI
    {
        public static string DummyQrCode;
        public static string DummyUserId = "0";
        public static bool QrLoginFlag = false;
        public static bool UserIdLoginFlag = false;

        private Rect PanelWindow;
        private string VersionText;
        private GUIStyle TextStyle;
        private GUIStyle BigTextStyle;
        private int ToolBarId = 0;
        private string[] ToolbarStrings = new string[]{ "AutoPlay", "FastSkip", "DummyLogin" };

        public ModGUI()
        {
            PanelWindow = new Rect();
            TextStyle = new GUIStyle();
            BigTextStyle = new GUIStyle();
            TextStyle.fontSize = 24;
            TextStyle.normal.textColor = Color.white;
            TextStyle.alignment = TextAnchor.UpperLeft;
            BigTextStyle.fontSize = 40;
            BigTextStyle.alignment = TextAnchor.MiddleCenter;
            BigTextStyle.normal.textColor = Color.white;
        }

        public void OnGUI()
        {
            PanelWindow = GUILayout.Window(10086, PanelWindow, ModPanel, $"{BuildInfo.Name} {BuildInfo.Version}");
            if (SinmaiAssist.config.ShowVersionInfo) ShowVersionInfo();
        }

        private void ModPanel(int id)
        {
            GUILayout.BeginVertical($"{BuildInfo.Name} {BuildInfo.Version}", GUILayout.Height(20f));
            ToolBarId = GUILayout.Toolbar(ToolBarId, ToolbarStrings, GUILayout.Width(300f), GUILayout.Height(20f));
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUILayout.Width(300f), GUILayout.Height(280f));
            switch (ToolBarId)
            {
                case 0:
                    if (!SinmaiAssist.config.AutoPlay) { GUILayout.Label("Disable", BigTextStyle, GUILayout.Width(300f), GUILayout.Height(280f)); break; }
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
                    break;
                case 1:
                    GUILayout.Label("前面的区域", BigTextStyle, GUILayout.Width(300f), GUILayout.Height(140f));
                    GUILayout.Label("以后再来探索吧", BigTextStyle, GUILayout.Width(300f), GUILayout.Height(140f));
                    break;
                case 2:
                    if (!SinmaiAssist.config.DummyLogin || !SinmaiAssist.IsSDGB) { GUILayout.Label("Disable", BigTextStyle, GUILayout.Width(300f), GUILayout.Height(280f)); break; }
                    GUILayout.Label("QrCode:");
                    DummyQrCode = GUILayout.TextArea(DummyQrCode, GUILayout.Height(100f));
                    if (GUILayout.Button("QrCode Login")) QrLoginFlag = true;
                    GUILayout.Space(10f);
                    GUILayout.Label("UserID:");
                    DummyUserId = GUILayout.TextField(DummyUserId, GUILayout.Height(20f));
                    if (GUILayout.Button("UserId Login")) UserIdLoginFlag = true;
                    break;
            }
            GUILayout.EndVertical();
            GUI.DragWindow(); 
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
            GUI.Label(new Rect(10,40,500,30), VersionText, TextStyle);
        }
    }
}
