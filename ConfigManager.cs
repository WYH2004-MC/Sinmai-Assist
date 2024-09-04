using Common;
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
        public bool SinglePlayer { get; private set; }
        public bool ForwardATouchRegionToButton { get; private set; }
        public bool ForceCurrentIsBest { get; private set; }
        public string CustomVersionText { get; private set; }
        public bool QuickBoot { get; private set; }
        public bool AutoPlay { get; private set; }
        public bool AllCollection {  get; private set; }
        public bool UnlockEvent { get; private set; }
        public bool DisableReboot { get; private set; }
        public bool SkipVersionCheck { get; private set; }
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
            SinglePlayer = iniFile.getValue("Common", "SinglePlayer", defaultParam: false);
            ForwardATouchRegionToButton = iniFile.getValue("Common", "ForwardATouchRegionToButton", defaultParam: false);
            ForceCurrentIsBest = iniFile.getValue("Common", "ForceCurrentIsBest", defaultParam: false);
            CustomVersionText = iniFile.getValue("Common", "CustomVersionText", defaultParam: null);
            QuickBoot = iniFile.getValue("Common", "QuickBoot", defaultParam: false);

            // [Cheat]
            AutoPlay = iniFile.getValue("Cheat", "AutoPlay", defaultParam: false);
            AllCollection = iniFile.getValue("Cheat", "AllCollection", defaultParam: false);
            UnlockEvent = iniFile.getValue("Cheat", "UnlockEvent", defaultParam: false);

            // [Fix]
            DisableReboot = iniFile.getValue("Fix", "DisableReboot", defaultParam: false);
            SkipVersionCheck = iniFile.getValue("Fix", "SkipVersionCheck", defaultParam: false);

            // [ModSetting]
            ShowVersionInfo = iniFile.getValue("ModSetting", "ShowVersionInfo", defaultParam: false);
            ForceIsSDGB = iniFile.getValue("ModSetting", "ForceIsSDGB", defaultParam: false);
            SafeMode = iniFile.getValue("ModSetting", "SafeMode", defaultParam: false);

        }
    }
}
