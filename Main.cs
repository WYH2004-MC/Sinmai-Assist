using Cheat;
using Common;
using Fix;
using MAI2System;
using MelonLoader;
using SDGB;
using System;
using System.IO;
using System.Reflection;
using Commmon;
using UnityEngine;

namespace SinmaiAssist
{
    public static class BuildInfo
    {
        public const string Name = "Sinmai-Assist";
        public const string Description = "SlimMod Melon Version For Sinmai";
        public const string Author = "Slim & Error063";
        public const string Company = null;
        public const string Version = "1.0.0";
        public const string DownloadLink = null;
        public const string BuildUserId = "0";
    }

    public class SinmaiAssist : MelonMod
    {
        private ModGUI modGUI;
        public static ConfigManager config;
        public static bool IsSDGB = false;
        public static string gameID = "Unknown";
        public static uint gameVersion = 00000;

        public override void OnInitializeMelon() {
            PrintLogo();
            modGUI = new ModGUI();
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
                gameID = (string) typeof(ConstParameter).GetField("GameIDStr", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).GetValue(null);
                gameVersion = (uint) typeof(ConstParameter).GetField("NowGameVersion", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).GetValue(null);
            }
            catch (Exception e) 
            {
                MelonLogger.Error("Failed to get GameIDStr and GameVersion");
                MelonLogger.Error(e);
            }
            if (gameID == "SDGB" || config.ModSetting.ForceIsChinaBuild) IsSDGB = true;
            MelonLogger.Msg($"GameInfo: {gameID} {gameVersion} - isSDGB: {IsSDGB}");

            if (config.ModSetting.SafeMode)
            {
                MelonLogger.Warning("Safe mode is enabled, Disable all patch");
                return;
            }

            // 加载Patch
            // SDGB
            if (config.China.DummyLogin.Enable) Patch(typeof(DummyLogin));
            if (config.China.CustomCameraId.Enable)
            {
                if (config.China.DummyLogin.Enable)
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
            if (config.Common.DisableMask) Patch(typeof(DisableMask));
            if (config.Common.SinglePlayer.Enable) Patch(typeof(SinglePlayer));
            if (config.Common.ForwardATouchRegionToButton) Patch(typeof(ForwardATouchRegionToButton));
            if (config.Common.QuickBoot) Patch(typeof(QuickBoot));
            if (config.Common.BlockCoin) Patch(typeof(BlockCoin));
            if (config.Common.SkipWarningScreen) Patch(typeof(SkipWarningScreen));
            if (config.Common.NetworkLogger.Enable) Patch(typeof(NetworkLogger));
            if (config.Common.CustomVersionText.Enable) Patch(typeof(CustomVersionText));

            //Fix
            if (config.Fix.DisableEncryption) Patch(typeof(DisableEncryption));
            if (config.Fix.DisableReboot) Patch (typeof(DisableReboot));
            if (config.Fix.SkipVersionCheck) Patch(typeof(SkipVersionCheck));
            if (config.Fix.RewriteNoteJudgeTiming.Enable) Patch(typeof(RewriteNoteJudgeTiming));

            //Cheat
            if (config.Cheat.AutoPlay) Patch(typeof(AutoPlay));
            if (config.Cheat.FastSkip) Patch(typeof(FastSkip));
            if (config.Cheat.ChartTimer) Patch(typeof(ChartTimer));
            if (config.Cheat.AllCollection) Patch(typeof(AllCollection));
            if (config.Cheat.UnlockEvent) Patch(typeof(UnlockEvent));
            if (config.Cheat.ResetLoginBonusRecord) Patch(typeof(ResetLoginBonusRecord));
            if (config.Cheat.ForceCurrentIsBest) Patch(typeof(ForceCurrentIsBest));
            if (config.Cheat.SetAllCharacterAsSameAndLock) Patch(typeof(SetAllCharacterAsSameAndLock));
            Patch(typeof(IgnoreAnyGameInformation));

            // 默认加载项
            Patch(typeof(FixDebugInput));
            Patch(typeof(PrintUserData));
            Patch(typeof(InputManager));

            MelonLogger.Msg("Loading completed");
        }

        public override void OnGUI()
        {
            modGUI.OnGUI();
            if (config.Common.ShowFPS) ShowFPS.OnGUI();
        }

        private static bool Patch(Type type)
        {
            try
            {
                MelonLogger.Msg($"- Patch: {type}");
                if (!IsSDGB && type.ToString().Contains("SDGB"))
                {
                    MelonLogger.Warning($"Patch {type} failed, because game is not SDGB. ");
                    return false;
                }
                HarmonyLib.Harmony.CreateAndPatchAll(type);
                return true;
            }
            catch (Exception e)
            {
                MelonLogger.Error($"Patch {type} failed.");
                MelonLogger.Error(e.StackTrace);
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
                $"\r\n Version: {BuildInfo.Version}     Author: {BuildInfo.Author}");
            MelonLogger.Warning("This is a cheat mod. Use at your own risk!");
        }
    }
}