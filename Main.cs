﻿using Cheat;
using Common;
using Fix;
using MAI2System;
using MelonLoader;
using SDGB;
using System;
using System.IO;
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
        public static string gameID = "";
        public static string gameVersion = "";

        public override void OnInitializeMelon() {
            PrintLogo();
            modGUI = new ModGUI();
            config = new ConfigManager();

            // 加载配置文件
            MelonLogger.Msg("Load Mod Config.");
            if (!File.Exists($"{BuildInfo.Name}/Config.ini"))
            {
                MelonLogger.Error($"Path: \"{BuildInfo.Name}/Config.ini\" Not Found.");
                return;
            }
            config.initialize();
            ModGUI.DummyUserId = config.DefaultDummyUserId;

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
            gameID = AMDaemon.System.GameId;
            if (gameID == "SDGB" || config.ForceIsSDGB) IsSDGB = true;
            MelonLogger.Msg($"GameId: {gameID} isSDGB: {IsSDGB}");

            if (config.SafeMode)
            {
                MelonLogger.Warning("Safe mode is enabled, Disable all patch");
                return;
            }

            // 加载Patch
            // SDGB
            if (config.DummyLogin) Patch(typeof(DummyLogin));
            if (config.CustomCameraId)
            {
                if (config.DummyLogin)
                {
                    MelonLogger.Warning("DummyLogin enabled, CustomCameraId has been automatically disabled.");
                }
                else
                {
                    Patch(typeof(CustomCameraId));
                }
            }

            // Common
            if (config.DisableMask) Patch(typeof(DisableMask));
            if (config.SinglePlayer) Patch(typeof(SinglePlayer));
            if (config.NetworkLogger) Patch(typeof(NetworkLogger));
            if (config.ForwardATouchRegionToButton) Patch(typeof(ForwardATouchRegionToButton));
            if (config.CustomVersionText != null) Patch(typeof(CustomVersionText));
            if (config.QuickBoot) Patch(typeof(QuickBoot));
            if (config.BlockCoin) Patch(typeof(BlockCoin));
            if (config.SkipWarningScreen) Patch(typeof(SkipWarningScreen));
            if (config.RewriteNoteJudgeSetting && (config.AdjustTiming != 0 || config.JudgeTiming != 0)) Patch(typeof(RewriteNoteJudgeSetting));

            //Fix
            if (config.DisableEncryption) Patch(typeof(DisableEncryption));
            if (config.DisableReboot) Patch (typeof(DisableReboot));
            if (config.SkipVersionCheck) Patch(typeof(SkipVersionCheck));

            //Cheat
            if (config.AutoPlay) Patch(typeof(AutoPlay));
            if (config.FastSkip) Patch(typeof(FastSkip));
            if (config.ChartTimer) Patch(typeof(ChartTimer));
            if (config.AllCollection) Patch(typeof(AllCollection));
            if (config.UnlockEvent) Patch(typeof(UnlockEvent));
            if (config.ResetLoginBonusRecord) Patch(typeof(ResetLoginBonusRecord));
            if (config.ForceCurrentIsBest) Patch(typeof(ForceCurrentIsBest));

            // 默认加载项
            Patch(typeof(FixDebugInput));
            Patch(typeof(PrintUserData));
            Patch(typeof(InputManager));

            MelonLogger.Msg("Loading completed");
        }

        public override void OnLateInitializeMelon()
        {
            gameVersion = ConstParameter.NowGameVersion.ToString();
        }

        public override void OnGUI()
        {
            modGUI.OnGUI();
            if (config.ShowFPS) ShowFPS.OnGUI();
        }

        private static bool Patch(Type type)
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