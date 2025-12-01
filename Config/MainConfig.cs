namespace SinmaiAssist.Config;

public class MainConfig
{
    public CommonConfig Common { get; set; } = new CommonConfig();
    public CheatConfig Cheat { get; set; } = new CheatConfig();
    public FixConfig Fix { get; set; } = new FixConfig();
    public ModSettingConfig ModSetting { get; set; } = new ModSettingConfig();

    public class CommonConfig
    {
        public UnityLoggerConfig UnityLogger { get; set; } = new UnityLoggerConfig();
        public bool AutoBackupData { get; set; } = false;
        public bool InfinityTimer { get; set; } = false;
        public bool InfinityTimerLegacy { get; set; } = false;
        public bool DisableMask { get; set; } = false;
        public bool DisableBackground { get; set; } = false;  
        public bool ShowFPS { get; set; } = true;  
        public bool ForceQuickRetry { get; set; } = false;  
        public bool ForwardATouchRegionToButton { get; set; } = false;  
        public bool SkipFade { get; set; } = false;  
        public bool SkipWarningScreen { get; set; } = false;  
        public bool QuickBoot { get; set; } = false;  
        public bool BlockCoin { get; set; } = false;  
        public bool IgnoreAnyGameInformation { get; set; } = false;  
        public bool ChangeDefaultOption { get; set; } = false;  
        public bool ChangeFadeStyle { get; set; } = false;
        public SinglePlayerConfig SinglePlayer { get; set; } = new SinglePlayerConfig();
        public NetworkLoggerConfig NetworkLogger { get; set; } = new NetworkLoggerConfig();
        public CustomVersionTextConfig CustomVersionText { get; set; } = new CustomVersionTextConfig();
        public DummyLoginConfig DummyLogin { get; set; } = new DummyLoginConfig();
        public CustomCameraIdConfig CustomCameraId { get; set; } = new CustomCameraIdConfig();
        public ChangeGameSettingsConfig ChangeGameSettings { get; set; } = new ChangeGameSettingsConfig();
    }
    
    public class CheatConfig
    {
        public bool AutoPlay { get; set; } = false;
        public bool FastSkip { get; set; } = false;
        public bool ChartController { get; set; } = false;
        public bool AllCollection { get; set; } = false;
        public bool UnlockEvent { get; set; } = false;
        public UnlockMusicConfig UnlockMusic { get; set; } = new UnlockMusicConfig();
        public UnlockMasterConfig UnlockMaster { get; set; } = new UnlockMasterConfig();
        public UnlockUtageConfig UnlockUtage { get; set; } = new UnlockUtageConfig();
        public bool ResetLoginBonusRecord { get; set; } = false;
        public bool ForceCurrentIsBest { get; set; } = false;
        public bool SetAllCharacterAsSameAndLock { get; set; } = false;
        public RewriteLoginBonusStampConfig RewriteLoginBonusStamp { get; set; } = new RewriteLoginBonusStampConfig();
    }

    public class FixConfig
    {
        public bool DisableEnvironmentCheck { get; set; } = true;
        public bool DisableEncryption { get; set; } = false;
        public bool DisableReboot { get; set; } = true;
        public bool DisableIniClear { get; set; } = true;
        public bool FixDebugInput { get; set; } = true;
        public bool FixCheckAuth { get; set; } = false;
        public bool ForceAsServer { get; set; } = false;
        public bool SkipCakeHashCheck { get; set; } = false;
        public bool SkipSpecialNumCheck { get; set; } = true;
        public bool SkipVersionCheck { get; set; } = false;
        public bool RestoreCertificateValidation { get; set; } = false;
        public RewriteNoteJudgeTimingConfig RewriteNoteJudgeTiming { get; set; } = new RewriteNoteJudgeTimingConfig();
    }
    
    public class ModSettingConfig
    {
        public bool SafeMode { get; set; } = false;
        public bool ShowInfo { get; set; } = true;
        public bool ShowPanel { get; set; } = true;
    }
    
    public class ChangeGameSettingsConfig
    {
        public bool Enable { get; set; } = false;
        public bool CodeRead { get; set; } = false;
        public bool IconPhoto { get; set; } = false;
        public bool UploadPhoto { get; set; } = false;
        public bool CharaSelect { get; set; } = false;
    }
    
    public class SinglePlayerConfig
    {
        public bool Enable { get; set; } = false;
        public bool HideSubMonitor { get; set; } = false;
    }

    public class NetworkLoggerConfig
    {
        public bool Enable { get; set; } = true;
        public bool PrintToConsole { get; set; } = false;
    }

    public class CustomVersionTextConfig
    {
        public bool Enable { get; set; } = false;
        public string VersionText { get; set; } = "Sinmai-Assist";
    }
    
    public class UnlockMusicConfig
    {
        public bool Enable { get; set; } = false;
        public bool SaveToUserData { get; set; } = false;
    }
    
    public class UnlockMasterConfig
    {
        public bool Enable { get; set; } = false;
        public bool SaveToUserData { get; set; } = false;
    }
    
    public class UnlockUtageConfig
    {
        public bool Enable { get; set; } = false;
        public bool UnlockDoublePlayerMusic { get; set; } = false;
    }

    public class RewriteNoteJudgeTimingConfig
    {
        public bool Enable { get; set; } = false;
        public float AdjustTiming { get; set; } = 0;
        public float JudgeTiming { get; set; } = 0;
    }
    
    public class DummyLoginConfig
    {
        public bool Enable { get; set; } = false;
        public int DefaultUserId { get; set; } = 0;
    }

    public class CustomCameraIdConfig
    {
        public bool Enable { get; set; } = false;
        public int ChimeCameraId { get; set; } = 0;
        public int LeftQrCameraId { get; set; } = 0;
        public int RightQrCameraId { get; set; } = 0;
        public int PhotoCameraId { get; set; } = 0;
    }

    public class RewriteLoginBonusStampConfig
    {
        public bool Enable { get; set; } = false;
        public uint Point { get; set; } = 0;
    } 

    public class UnityLoggerConfig
    {
        public bool Enable { get; set; } = true;
        public bool PrintToConsole { get; set; } = true;
    }
}