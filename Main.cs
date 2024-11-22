using MAI2System;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SinmaiAssist.Cheat;
using SinmaiAssist.Common;
using SinmaiAssist.Fix;
using SinmaiAssist.GUI;
using SinmaiAssist.Utils;
using UnityEngine;
using Path = System.IO.Path;

namespace SinmaiAssist
{
    public static partial class BuildInfo
    {
        public const string Name = "Sinmai-Assist";
        public const string Description = "SlimMod Melon Version For Sinmai";
        public const string Author = "Slim & Error063";
        public const string Company = null;
        public const string Version = "1.0.0";
        public const string DownloadLink = null;
    }

    public class SinmaiAssist : MelonMod
    {
        private MainGUI _mainGUI;
        private static bool isPatchFailed = false;
        public static ConfigManager config;
        public static string gameID = "Unknown";
        public static uint gameVersion = 00000;

        public override void OnInitializeMelon()
        {
            File.Delete($"{BuildInfo.Name}/Unity.log");
            Application.logMessageReceived += OnLogMessageReceived; // 注册Unity日志
            PrintLogo();
            _mainGUI = new MainGUI();
            config = new ConfigManager();

            // 加载配置文件
            MelonLogger.Msg("Load Mod Config.");
            string yamlFilePath = $"{BuildInfo.Name}/config.yml";
            if (!File.Exists(yamlFilePath))
            {
                MelonLogger.Error($"Path: \"{yamlFilePath}\" Not Found.");
                return;
            }

            try
            {
                config.Initialize(yamlFilePath);
                DummyLoginPanel.DummyUserId = config.Common.DummyLogin.DefaultUserId.ToString();
                DebugPanel.UnityLogger = config.ModSetting.LogUnity;
                MelonLogger.Msg("Config Load Complete.");
            }
            catch (Exception e)
            {
                MelonLogger.Error($"Error initializing mod config: \n{e}");
                return;
            }


            // 输出设备摄像头列表
            File.Delete($"{BuildInfo.Name}/WebCameraList.txt");
            WebCamDevice[] devices = WebCamTexture.devices;
            string CameraList = "\nConnected Web Cameras:\n";
            for (int i = 0; i < devices.Length; i++)
            {
                WebCamDevice webCamDevice = devices[i];
                CameraList = CameraList + "Name: " + webCamDevice.name + "\n";
                CameraList += $"ID: {i}\n";
                WebCamTexture webCamTexture = new WebCamTexture(webCamDevice.name);
                webCamTexture.Play();
                CameraList += $"Resolution: {webCamTexture.width}x{webCamTexture.height}\n";
                CameraList += $"FPS: {webCamTexture.requestedFPS}\n";
                webCamTexture.Stop();
                CameraList += "\n";
            }

            File.AppendAllText($"{BuildInfo.Name}/WebCameraList.txt", CameraList);

            // 检测游戏版本并判断是否为 SDGB
            try
            {
                gameID = (string)typeof(ConstParameter).GetField("GameIDStr",
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).GetValue(null);
                gameVersion = (uint)typeof(ConstParameter).GetField("NowGameVersion",
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).GetValue(null);

                if (gameVersion < 24000)
                {
                    MelonLogger.Warning($"Using untested version ({gameVersion}) maybe case some unexcepted problems!");
                }
            }
            catch (Exception e)
            {
                MelonLogger.Error("Failed to get GameIDStr and GameVersion");
                MelonLogger.Error(e);
            }
            
            MelonLogger.Msg($"GameInfo: {gameID} {gameVersion} ");
            var Codes = new List<int> { 83, 68, 71, 66 };
            var str = string.Concat(Codes.Select(code => (char)code));
            if (gameID.Equals(str)) Application.Quit();

            if (config.ModSetting.SafeMode)
            {
                MelonLogger.Warning("Safe mode is enabled, Disable all patch");
                return;
            }

            // 加载Patch
            // DummyLogin
            if (config.Common.DummyLogin.Enable)
            {
                if (File.Exists("DEVICE/aime.txt"))
                    DummyLoginPanel.DummyLoginCode = File.ReadAllText("DEVICE/aime.txt").Trim();
                    Patch(typeof(DummyAimeLogin));
            }
            
            if (config.Common.CustomCameraId.Enable)
            {
                if (config.Common.DummyLogin.Enable)
                {
                    MelonLogger.Warning("DummyLogin enabled, CustomCameraId has been automatically disabled.");
                }
                else
                {
                    Patch(typeof(CustomCameraId));
                }
            }

            // Common
            if (config.Common.InfinityTimer) Patch(typeof(InfinityTimer));
            if (config.Common.DisableBackground) Patch(typeof(DisableBackground));
            if (config.Common.DisableMask) Patch(typeof(DisableMask));
            if (config.Common.SinglePlayer.Enable) Patch(typeof(SinglePlayer));
            if (config.Common.ForceQuickRetry) Patch(typeof(ForceQuickRetry));
            if (config.Common.ForwardATouchRegionToButton) Patch(typeof(ForwardATouchRegionToButton));
            if (config.Common.QuickBoot) Patch(typeof(QuickBoot));
            if (config.Common.BlockCoin) Patch(typeof(BlockCoin));
            if (config.Common.SkipWarningScreen) Patch(typeof(SkipWarningScreen));
            if (config.Common.SkipFade) Patch(typeof(SkipFade));
            if (config.Common.NetworkLogger.Enable) Patch(typeof(NetworkLogger));
            if (config.Common.CustomVersionText.Enable) Patch(typeof(CustomVersionText));
            if (config.Common.IgnoreAnyGameInformation) Patch(typeof(IgnoreAnyGameInformation));
            if (config.Common.ChangeDefaultOption) Patch(typeof(ChangeDefaultOption));
            if (config.Common.ChangeFadeStyle) Patch(typeof(ChangeFadeStyle));
            if (config.Common.ChangeGameSettings.Enable) Patch(typeof(ChangeGameSettings));

            //Fix
            if (config.Fix.DisableEncryption) Patch(typeof(DisableEncryption));
            if (config.Fix.DisableReboot) Patch(typeof(DisableReboot));
            if (config.Fix.SkipVersionCheck) Patch(typeof(SkipVersionCheck));
            if (config.Fix.RewriteNoteJudgeTiming.Enable) Patch(typeof(RewriteNoteJudgeTiming));

            //Cheat
            if (config.Cheat.AutoPlay) Patch(typeof(AutoPlay));
            if (config.Cheat.FastSkip) Patch(typeof(FastSkip));
            if (config.Cheat.ChartTimer) Patch(typeof(ChartTimer));
            if (config.Cheat.AllCollection) Patch(typeof(AllCollection));
            if (config.Cheat.UnlockMusic) Patch(typeof(UnlockMusic));
            if (config.Cheat.UnlockMaster) Patch(typeof(UnlockMaster));
            if (config.Cheat.UnlockEvent) Patch(typeof(UnlockEvent));
            if (config.Cheat.ResetLoginBonusRecord) Patch(typeof(ResetLoginBonusRecord));
            if (config.Cheat.ForceCurrentIsBest) Patch(typeof(ForceCurrentIsBest));
            if (config.Cheat.SetAllCharacterAsSameAndLock) Patch(typeof(SetAllCharacterAsSameAndLock));
            if (config.Cheat.RewriteLoginBonusStamp.Enable) Patch(typeof(RewriteLoginBonusStamp));

            // 默认加载项
            Patch(typeof(FixDebugInput));
            Patch(typeof(PrintUserData));
            Patch(typeof(InputManager));
            Patch(typeof(GameMessageManager));
            
            if(isPatchFailed) PatchFailedWarn();
            MelonLogger.Msg("Loading completed");
        }

        public override void OnGUI()
        {
            _mainGUI.OnGUI();
            if (config.Common.ShowFPS) ShowFPS.OnGUI();
            if (config.ModSetting.ShowInfo) ShowVersionInfo.OnGUI();
        }
        
        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            string logString = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{type}] {condition}\n{stackTrace}";

            File.AppendAllText(Path.Combine($"{BuildInfo.Name}/Unity.log"),logString + "\n");
            if (DebugPanel.UnityLogger)
            {
                switch (type)
                {
                    case LogType.Error:
                    case LogType.Exception:
                        MelonLogger.Error($"[UnityLogger] [{type}]: {condition}\n{stackTrace}");
                        break;
                    case LogType.Warning:
                        MelonLogger.Warning($"[UnityLogger] [{type}]: {condition}\n{stackTrace}");
                        break;
                    case LogType.Assert:
                    case LogType.Log:
                    default:
                        MelonLogger.Msg($"[UnityLogger] [{type}]: {condition}\n{stackTrace}");
                        break;
                }
            }
        }

        private static bool Patch(Type type)
        {
            try
            {
                MelonLogger.Msg($"- Patch: {type}");

                HarmonyLib.Harmony.CreateAndPatchAll(type);
                return true;
            }
            catch (Exception e)
            {
                MelonLogger.Error($"Patch {type} failed.");
                MelonLogger.Error(e.Message);
                MelonLogger.Error(e.Source);
                MelonLogger.Error(e.TargetSite);
                MelonLogger.Error(e.InnerException);
                MelonLogger.Error(e.StackTrace);
                isPatchFailed = true;
                return false;
            }
        }

        private static void PrintLogo()
        {
            MelonLogger.Msg("\n" +
                            "\r\n   _____ _       __  ___      _       ___              _      __ " +
                            "\r\n  / ___/(_)___  /  |/  /___ _(_)     /   |  __________(_)____/ /_" +
                            "\r\n  \\__ \\/ / __ \\/ /|_/ / __ `/ /_____/ /| | / ___/ ___/ / ___/ __/" +
                            "\r\n ___/ / / / / / /  / / /_/ / /_____/ ___ |(__  |__  ) (__  ) /_  " +
                            "\r\n/____/_/_/ /_/_/  /_/\\__,_/_/     /_/  |_/____/____/_/____/\\__/  " +
                            "\r\n                                                                 " +
                            "\r\n=================================================================" +
                            $"\r\n Version: {BuildInfo.Version} ({BuildInfo.CommitHash}) Build Date: {BuildInfo.BuildDate}" +
                            $"\r\n Author: {BuildInfo.Author}");
            MelonLogger.Warning("This is a cheat mod. Use at your own risk!");
        }

        private static void PatchFailedWarn()
        {
            MelonLogger.Warning("\r\n=================================================================" +
                                "\r\nFailed to patch some methods." +
                                "\r\nPlease ensure that you are using an unmodified version of Assembly-CSharp.dll with a version greater than 1.40.0," +
                                "\r\nas modifications or lower versions can cause function mismatches, preventing the required functions from being found." +
                                "\r\nCheck for conflicting mods, or enabled incompatible options." +
                                "\r\nIf you believe this is an error, please report the issue to the mod author." +
                                "\r\n=================================================================");
        }
    }
}