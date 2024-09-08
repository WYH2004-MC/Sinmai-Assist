﻿using Cheat;
using Common;
using Fix;
using MAI2System;

namespace SinmaiAssist
{
    public class ConfigManager
    {
        public bool DummyLogin { get; private set; }
        public string DefaultDummyUserId { get; private set; }
        public bool CustomCameraId { get; private set; }
        public int CustomQrCameraId { get; private set; }
        public int CustomPhotoCameraId { get; private set; }
        public bool DisableMask { get; private set; }
        public bool ShowFPS { get; private set; }
        public bool NetworkLogger {  get; private set; }
        public bool NetworkLoggerPrintToConsole { get; private set; }
        public bool SinglePlayer { get; private set; }
        public bool ForwardATouchRegionToButton { get; private set; }
        public string CustomVersionText { get; private set; }
        public bool SkipWarningScreen { get; private set; }
        public bool QuickBoot { get; private set; }
        public bool BlockCoin { get; private set; }
        public bool AutoPlay { get; private set; }
        public bool FastSkip { get; private set; }
        public bool AllCollection {  get; private set; }
        public bool UnlockEvent { get; private set; }
        public bool ResetLoginBonusRecord { get; private set; }
        public bool ForceCurrentIsBest { get; private set; }
        public bool DisableEncryption { get; private set; }
        public bool DisableReboot { get; private set; }
        public bool SkipVersionCheck { get; private set; }
        public bool RewriteNoteJudgeSetting { get; private set; }
        public float AdjustTiming { get; private set; }
        public float JudgeTiming { get; private set; }
        public bool ShowVersionInfo { get; private set; }
        public bool ForceIsSDGB { get; private set; }
        public bool SafeMode { get; private set; }


        public void initialize()
        {
            IniFile iniFile = new IniFile($"{BuildInfo.Name}/Config.ini");
            // [SDGB]
            DummyLogin = iniFile.getValue("SDGB", "DummyLogin", defaultParam: false);
            DefaultDummyUserId = iniFile.getValue("SDGB", "DefaultDummyUserId", defaultParam: "0");
            CustomCameraId = iniFile.getValue("SDGB", "CustomCameraId", defaultParam: false);
            CustomQrCameraId = iniFile.getValue("SDGB", "CustomQrCameraId", defaultParam: 0);
            CustomPhotoCameraId = iniFile.getValue("SDGB", "CustomPhotoCameraId", defaultParam: 0);

            // [Common]
            DisableMask = iniFile.getValue("Common", "DisableMask", defaultParam: false);
            ShowFPS = iniFile.getValue("Common", "ShowFPS", defaultParam: false);
            NetworkLogger = iniFile.getValue("Common", "NetworkLogger", defaultParam: false);
            NetworkLoggerPrintToConsole = iniFile.getValue("Common", "NetworkLoggerPrintToConsole", defaultParam: false);
            SinglePlayer = iniFile.getValue("Common", "SinglePlayer", defaultParam: false);
            ForwardATouchRegionToButton = iniFile.getValue("Common", "ForwardATouchRegionToButton", defaultParam: false);
            CustomVersionText = iniFile.getValue("Common", "CustomVersionText", defaultParam: null);
            SkipWarningScreen = iniFile.getValue("Common", "SkipWarningScreen", defaultParam: false);
            QuickBoot = iniFile.getValue("Common", "QuickBoot", defaultParam: false);
            BlockCoin = iniFile.getValue("Common", "BlockCoin", defaultParam: false);
            
            // [Cheat]
            AutoPlay = iniFile.getValue("Cheat", "AutoPlay", defaultParam: false);
            FastSkip = iniFile.getValue("Cheat", "FastSkip", defaultParam: false);
            AllCollection = iniFile.getValue("Cheat", "AllCollection", defaultParam: false);
            UnlockEvent = iniFile.getValue("Cheat", "UnlockEvent", defaultParam: false);
            ResetLoginBonusRecord = iniFile.getValue("Cheat", "ResetLoginBonusRecord", defaultParam: false);
            ForceCurrentIsBest = iniFile.getValue("Cheat", "ForceCurrentIsBest", defaultParam: false);

            // [Fix]
            DisableEncryption = iniFile.getValue("Fix", "DisableEncryption", defaultParam: false);
            DisableReboot = iniFile.getValue("Fix", "DisableReboot", defaultParam: false);
            SkipVersionCheck = iniFile.getValue("Fix", "SkipVersionCheck", defaultParam: false);
            RewriteNoteJudgeSetting = iniFile.getValue("Fix", "RewriteNoteJudgeSetting", defaultParam: false);
            AdjustTiming = iniFile.getValue("Fix", "AdjustTiming", defaultParam: 0f);
            JudgeTiming = iniFile.getValue("Fix", "JudgeTiming", defaultParam: 0f);

            // [ModSetting]
            ShowVersionInfo = iniFile.getValue("ModSetting", "ShowVersionInfo", defaultParam: false);
            ForceIsSDGB = iniFile.getValue("ModSetting", "ForceIsSDGB", defaultParam: false);
            SafeMode = iniFile.getValue("ModSetting", "SafeMode", defaultParam: false);

        }
    }
}
