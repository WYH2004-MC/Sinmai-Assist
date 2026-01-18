using MAI2System;
using MelonLoader;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using HarmonyLib;
using SinmaiAssist.Attributes;
using SinmaiAssist.Cheat;
using SinmaiAssist.Common;
using SinmaiAssist.Config;
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
        public const string Author = "WYH2004 (Slim) & Error063";
        public const string Company = "Wahleak Technology()";
        public const string Version = "1.0.0";
        public const string DownloadLink = null;
    }

    public class SinmaiAssist : MelonMod
    {
        private MainGUI _mainGUI;
        private static bool _isPatchFailed = false;
        private static ConfigManager<MainConfig> _mainConfigManager;
        private static ConfigManager<KeyBindConfig> _keyBindConfigManager;
        public static MainConfig MainConfig;
        public static KeyBindConfig KeyBindConfig;
        public static string GameID = "Unknown";
        public static uint GameVersion = 00000;

        public override void OnInitializeMelon()
        {
            if(!Directory.Exists($"./{BuildInfo.Name}")) Directory.CreateDirectory($"./{BuildInfo.Name}");
            
            // 初始化UnityLogger
            if(File.Exists($"./{BuildInfo.Name}/Unity.log"))  File.Delete($"{BuildInfo.Name}/Unity.log"); 
            File.WriteAllText($"./{BuildInfo.Name}/Unity.log", "");
            Application.logMessageReceived += OnLogMessageReceived; 
            
            PrintLogo();
            _mainGUI = new MainGUI();
            
            // 加载配置文件
            try
            {
                var keyBindCoverter = new KeyBindConfig.Converter();
                _mainConfigManager = new ConfigManager<MainConfig>($"./{BuildInfo.Name}/Config.yml");
                _keyBindConfigManager = new ConfigManager<KeyBindConfig>($"./{BuildInfo.Name}/KeyBindConfig.yml", keyBindCoverter);
                MainConfig = _mainConfigManager.GetConfig();
                KeyBindConfig = _keyBindConfigManager.GetConfig();
                DummyLoginPanel.DummyUserId = MainConfig.Common.DummyLogin.DefaultUserId.ToString();
                DebugPanel.UnityLogger = MainConfig.Common.UnityLogger.Enable;
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
            
            try
            {
                GameID = (string)typeof(ConstParameter).GetField("GameIDStr",
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).GetValue(null);
                GameVersion = (uint)typeof(ConstParameter).GetField("NowGameVersion",
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).GetValue(null);

                if (GameVersion < 24000)
                {
                    MelonLogger.Warning($"Using untested version ({GameVersion}) maybe case some unexcepted problems!");
                }
            }
            catch (Exception e)
            {
                MelonLogger.Error("Failed to get GameIDStr and GameVersion");
                MelonLogger.Error(e);
            }
            
            MelonLogger.Msg($"GameInfo: {GameID} {GameVersion} ");
            
            // 弃用的神秘判断代码
            // var Codes = new List<int> { 83, 68, 71, 66 };
            // var str = string.Concat(Codes.Select(code => (char)code));

            if (MainConfig.ModSetting.SafeMode)
            {
                MelonLogger.Warning("Safe mode is enabled, Disable all patch");
                return;
            }

            // 加载Patch
            Patch(typeof(HarmonyLibPatch),true);
            
            // DummyLogin
            if (MainConfig.Common.DummyLogin.Enable)
            {
                if (GameID.Equals("SDGB"))
                {
                    Patch(typeof(DummyChimeLogin));
                }
                else
                {
                    if (File.Exists("DEVICE/aime.txt"))
                        DummyLoginPanel.DummyLoginCode = File.ReadAllText("DEVICE/aime.txt").Trim();
                    Patch(typeof(DummyAimeLogin));
                }
            }
            
            if (MainConfig.Common.CustomCameraId.Enable)
            {
                if (MainConfig.Common.DummyLogin.Enable)
                {
                    MelonLogger.Warning("DummyLogin enabled, CustomCameraId has been automatically disabled.");
                }
                else
                {
                    Patch(typeof(CustomCameraId));
                }
            }

            // Common
            if (MainConfig.Common.AutoBackupData) Patch(typeof(AutoBackupData));
            if (MainConfig.Common.InfinityTimer) Patch(typeof(InfinityTimer));
            if (MainConfig.Common.InfinityTimerLegacy) Patch(typeof(InfinityTimerLegacy));
            if (MainConfig.Common.DisableBackground) Patch(typeof(DisableBackground));
            if (MainConfig.Common.DisableMask) Patch(typeof(DisableMask));
            if (MainConfig.Common.SinglePlayer.Enable) Patch(typeof(SinglePlayer));
            if (MainConfig.Common.ForceQuickRetry) Patch(typeof(ForceQuickRetry));
            if (MainConfig.Common.ForwardATouchRegionToButton) Patch(typeof(ForwardATouchRegionToButton));
            // 存在资源加载问题，现已禁用
            // if (MainConfig.Common.QuickBoot) Patch(typeof(QuickBoot)); 
            if (MainConfig.Common.BlockCoin) Patch(typeof(BlockCoin));
            if (MainConfig.Common.SkipWarningScreen) Patch(typeof(SkipWarningScreen));
            if (MainConfig.Common.SkipFade) Patch(typeof(SkipFade));
            if (MainConfig.Common.NetworkLogger.Enable) Patch(typeof(NetworkLogger));
            if (MainConfig.Common.CustomVersionText.Enable) Patch(typeof(CustomVersionText));
            if (MainConfig.Common.IgnoreAnyGameInformation) Patch(typeof(IgnoreAnyGameInformation));
            if (MainConfig.Common.ChangeDefaultOption) Patch(typeof(ChangeDefaultOption));
            if (MainConfig.Common.ChangeFadeStyle) Patch(typeof(ChangeFadeStyle));
            if (MainConfig.Common.ChangeGameSettings.Enable) Patch(typeof(ChangeGameSettings));

            //Fix
            if (MainConfig.Fix.DisableEnvironmentCheck) Patch(typeof(DisableEnvironmentCheck));
            if (MainConfig.Fix.DisableEncryption) Patch(typeof(DisableEncryption));
            if (MainConfig.Fix.DisableReboot) Patch(typeof(DisableReboot));
            if (MainConfig.Fix.DisableIniClear) Patch(typeof(DisableIniClear));
            if (MainConfig.Fix.FixDebugInput) Patch(typeof(FixDebugInput));
            if (MainConfig.Fix.FixCheckAuth) Patch(typeof(FixCheckAuth));
            if (MainConfig.Fix.ForceAsServer) Patch(typeof(ForceAsServer));
            if (MainConfig.Fix.SkipCakeHashCheck) Patch(typeof(SkipCakeHashCheck));
            if (MainConfig.Fix.SkipSpecialNumCheck) Patch(typeof(SkipSpecialNumCheck));
            if (MainConfig.Fix.SkipVersionCheck) Patch(typeof(SkipVersionCheck));
            if (MainConfig.Fix.RestoreCertificateValidation) Patch(typeof(RestoreCertificateValidation));
            if (MainConfig.Fix.RewriteNoteJudgeTiming.Enable) Patch(typeof(RewriteNoteJudgeTiming));

            //Cheat
            if (MainConfig.Cheat.AutoPlay) Patch(typeof(AutoPlay));
            if (MainConfig.Cheat.FastSkip) Patch(typeof(FastSkip));
            if (MainConfig.Cheat.ChartController) Patch(typeof(ChartController));
            if (MainConfig.Cheat.AllCollection) Patch(typeof(AllCollection));
            if (MainConfig.Cheat.UnlockMusic.Enable) Patch(typeof(UnlockMusic));
            if (MainConfig.Cheat.UnlockMaster.Enable) Patch(typeof(UnlockMaster));
            if (MainConfig.Cheat.UnlockUtage.Enable) Patch(typeof(UnlockUtage));
            if (MainConfig.Cheat.UnlockEvent) Patch(typeof(UnlockEvent));
            if (MainConfig.Cheat.ResetLoginBonusRecord) Patch(typeof(ResetLoginBonusRecord));
            if (MainConfig.Cheat.ForceCurrentIsBest) Patch(typeof(ForceCurrentIsBest));
            if (MainConfig.Cheat.SetAllCharacterAsSameAndLock) Patch(typeof(SetAllCharacterAsSameAndLock));
            if (MainConfig.Cheat.RewriteLoginBonusStamp.Enable) Patch(typeof(RewriteLoginBonusStamp));

            // 默认加载项
            Patch(typeof(PrintUserData));
            Patch(typeof(InputManager));
            Patch(typeof(GameMessageManager));
            
            if(_isPatchFailed) PatchFailedWarn();
            MelonLogger.Msg("Loading completed");
        }
        public override void OnApplicationQuit()
        {
            
        }

        public override void OnGUI()
        {
            _mainGUI.OnGUI();
            if (MainConfig.Common.ShowFPS) ShowFPS.OnGUI();
            if (MainConfig.ModSetting.ShowInfo) ShowVersionInfo.OnGUI();
        }
        
        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            string logString = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{type}] {condition}\n{stackTrace}";

            if (MainConfig.Common.UnityLogger.Enable) File.AppendAllText(Path.Combine($"{BuildInfo.Name}/Unity.log"),logString + "\n");
            if (MainConfig.Common.UnityLogger.Enable && MainConfig.Common.UnityLogger.PrintToConsole)
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

        private static bool Patch(Type type, bool noLoggerPrint = false)
        {
            try
            {
                // var enableGameVersion = type.GetCustomAttribute<EnableGameVersionAttribute>();
                // if (enableGameVersion != null && !enableGameVersion.ShouldEnable())
                // {
                //     MelonLogger.Warning(
                //         $"Patch: {type} skipped ,Game version need Min {enableGameVersion.MinGameVersion} Max {enableGameVersion.MaxGameVersion}");
                //     return false;
                // }

                if (!noLoggerPrint) MelonLogger.Msg($"> Patch: {type}");
                HarmonyLib.Harmony.CreateAndPatchAll(type);
                return true;
            }
            catch (Exception e)
            {
                MelonLogger.Error($"Patch: {type} failed.");
                MelonLogger.Error(e.Message);
                MelonLogger.Error(e.Source);
                MelonLogger.Error(e.TargetSite);
                MelonLogger.Error(e.InnerException);
                MelonLogger.Error(e.StackTrace);
                _isPatchFailed = true;
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
            MelonLogger.Warning("\n" +
                                "\r\n=================================================================" +
                                "\r\nThis is a cheat mod. Use at your own risk!" +
                                "\r\nThis is a free and open-source mod. Resale is strictly prohibited." +
                                "\r\nIf you paid for this mod, you are stupid." +
                                "\r\n================================================================="
                                );
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
    public class HarmonyLibPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("HarmonyLib.PatchTools", "GetPatchMethod")]
        public static void GetPatchMethod(ref MethodInfo __result)
        {
            if (__result != null)
            {
                var enableGameVersion = __result.GetCustomAttribute<EnableGameVersionAttribute>();
                if (enableGameVersion != null && !enableGameVersion.ShouldEnable())
                {
#if DEBUG
                    MelonLogger.Warning($" Patch: {__result.ReflectedType}.{__result.Name} skipped, Game version need Min {enableGameVersion.MinGameVersion} Max {enableGameVersion.MaxGameVersion}");
#endif
                    __result = null;
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("HarmonyLib.PatchTools", "GetPatchMethods")]
        public static void GetPatchMethods(ref IList __result)
        {
            for (int i = 0; i < __result.Count; i++)
            {
                var harmonyMethod = Traverse.Create(__result[i]).Field("info").GetValue() as HarmonyMethod;
                var method = harmonyMethod.method;
                var enableGameVersion = method.GetCustomAttribute<EnableGameVersionAttribute>();
                if (enableGameVersion != null && !enableGameVersion.ShouldEnable())
                {
#if DEBUG
                    MelonLogger.Warning($" Patch: {method.ReflectedType}.{method.Name} skipped, Game version need Min {enableGameVersion.MinGameVersion} Max {enableGameVersion.MaxGameVersion}");
#endif
                    __result.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
