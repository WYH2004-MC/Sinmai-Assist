using Cheat;
using MAI2.Util;
using MAI2System;
using System;
using System.Text;
using Manager;
using Manager.UserDatas;
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
        public static string achivementInput = "0";
        public static bool QrLoginFlag = false;
        public static bool UserIdLoginFlag = false;

        private Rect PanelWindow;
        private StringBuilder VersionText = new StringBuilder();
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
            TextStyle = new GUIStyle();
            TextShadowStyle = new GUIStyle();
            BigTextStyle = new GUIStyle();
            errorStyle = new GUIStyle();
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
            else if (toolbar == Toolbar.FastSkip && SinmaiAssist.config.FastSkip) FastSkipPanel();
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
            FastSkip.SkipButton = false;
            GUILayout.Label($"Skip Mode: {(FastSkip.CustomSkip ? "Custom" : "Default")}");
            if (GUILayout.Button("Skip", new GUIStyle(GUI.skin.button){ fontSize=20 }, GUILayout.Height(45f))) FastSkip.SkipButton = true;
            GUILayout.Label($"Mode Setting", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
            if (GUILayout.Button("Default")) FastSkip.CustomSkip = false;
            if (GUILayout.Button("Custom")) FastSkip.CustomSkip = true;
            if (FastSkip.CustomSkip)
            {
                GUILayout.Label($"Custom Setting", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
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
            VersionText.Clear();
            VersionText.AppendLine($"{BuildInfo.Name} {BuildInfo.Version}");
            VersionText.AppendLine("Powered by MelonLoader");
            VersionText.AppendLine($"Client Version: {SinmaiAssist.gameID} {SinmaiAssist.gameVersion}");
            VersionText.AppendLine($"Current Title Server: {Singleton<OperationManager>.Instance.GetBaseUri()}");
            VersionText.AppendLine(
                $"Data Version: {Singleton<SystemConfig>.Instance.config.dataVersionInfo.versionNo.versionString} {Singleton<SystemConfig>.Instance.config.dataVersionInfo.versionNo.releaseNoAlphabet}");
            VersionText.AppendLine($"Keychip: {AMDaemon.System.KeychipId}");
            VersionText.AppendLine($"UserId: {Singleton<UserDataManager>.Instance.GetUserData(0L).Detail.UserID} | {Singleton<UserDataManager>.Instance.GetUserData(1L).Detail.UserID}");
            GUI.Label(new Rect(10+2, 40+2, 500, 30), VersionText.ToString(), TextShadowStyle);
            GUI.Label(new Rect(10, 40, 500, 30), VersionText.ToString(), TextStyle);
        }
    }
}
