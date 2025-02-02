using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SinmaiAssist
{
    public class ConfigManager
    {
        private Config _config;

        // 初始化方法，加载配置文件
        public void Initialize(string yamlFilePath)
        {
            var deserializer = new DeserializerBuilder()
                  .WithNamingConvention(CamelCaseNamingConvention.Instance)
                  .Build();

            _config = deserializer.Deserialize<Config>(File.ReadAllText(yamlFilePath));
        }

        // 提供公共属性访问
        public CommonConfig Common => _config.Common;
        public CheatConfig Cheat => _config.Cheat;
        public FixConfig Fix => _config.Fix;
        public ModSettingConfig ModSetting => _config.ModSetting;

        public string GetConfigAsYaml()
        {
            var serializer = new SerializerBuilder().Build();
            return serializer.Serialize(_config);
        }
    }

    public class Config
    {
        public CommonConfig Common { get; set; }
        public CheatConfig Cheat { get; set; }
        public FixConfig Fix { get; set; }
        public ModSettingConfig ModSetting { get; set; }
    }
    
    public class DummyLoginConfig
    {
        public bool Enable { get; set; }
        public int DefaultUserId { get; set; }
    }

    public class CustomCameraIdConfig
    {
        public bool Enable { get; set; }
        public int ChimeCameraId { get; set; }
        public int LeftQrCameraId { get; set; }
        public int RightQrCameraId { get; set; }
        public int PhotoCameraId { get; set; }
    }

    public class CommonConfig
    {
        public bool InfinityTimer { get; set; }
        public bool InfinityTimerLegacy { get; set; }
        public bool DisableMask { get; set; }
        public bool DisableBackground { get; set; }
        public bool ShowFPS { get; set; }
        public bool ForceQuickRetry { get; set; }
        public bool ForwardATouchRegionToButton { get; set; }
        public bool SkipFade { get; set; }
        public bool SkipWarningScreen { get; set; }
        public bool QuickBoot { get; set; }
        public bool BlockCoin { get; set; }
        public bool IgnoreAnyGameInformation { get; set; }
        public bool ChangeDefaultOption { get; set; }
        public bool ChangeFadeStyle { get; set; }
        public SinglePlayerConfig SinglePlayer { get; set; }
        public NetworkLoggerConfig NetworkLogger { get; set; }
        public CustomVersionTextConfig CustomVersionText { get; set; }
        public DummyLoginConfig DummyLogin { get; set; }
        public CustomCameraIdConfig CustomCameraId { get; set; }
        public ChangeGameSettingsConfig ChangeGameSettings { get; set; }
    }

    public class SinglePlayerConfig
    {
        public bool Enable { get; set; }
        public bool HideSubMonitor { get; set; }
    }

    public class NetworkLoggerConfig
    {
        public bool Enable { get; set; }
        public bool PrintToConsole { get; set; }
    }

    public class CustomVersionTextConfig
    {
        public bool Enable { get; set; }
        public string VersionText { get; set; }
    }

    public class ChangeGameSettingsConfig
    {
        public bool Enable { get; set; }
        public bool CodeRead { get; set; }
        public bool IconPhoto { get; set; }
        public bool UploadPhoto { get; set; }
        public bool CharaSelect { get; set; }
    }

    public class CheatConfig
    {
        public bool AutoPlay { get; set; }
        public bool FastSkip { get; set; }
        public bool ChartTimer { get; set; }
        public bool AllCollection { get; set; }
        public bool UnlockEvent { get; set; }
        public bool UnlockMusic { get; set; }
        public bool UnlockMaster { get; set; }
        public bool SaveUnlockMusic { get; set; }
        public bool SaveUnlockMaster { get; set; }
        public bool ResetLoginBonusRecord { get; set; }
        public bool ForceCurrentIsBest { get; set; }
        public bool SetAllCharacterAsSameAndLock { get; set; }
        public RewriteLoginBonusStampConfig RewriteLoginBonusStamp { get; set; }
    }

    public class FixConfig
    {
        public bool DisableEnvironmentCheck { get; set; }
        public bool DisableEncryption { get; set; }
        public bool DisableReboot { get; set; }
        public bool SkipVersionCheck { get; set; }
        public RewriteNoteJudgeTimingConfig RewriteNoteJudgeTiming { get; set; }
    }

    public class RewriteNoteJudgeTimingConfig
    {
        public bool Enable { get; set; }
        public float AdjustTiming { get; set; }
        public float JudgeTiming { get; set; }
    }

    public class ModSettingConfig
    {
        public bool ShowInfo { get; set; }
        public bool ShowPanel { get; set; }
        public bool LogUnity { get; set; }
        public bool SafeMode { get; set; }
    }

    public class RewriteLoginBonusStampConfig
    {
        public bool Enable { get; set; }

        private uint _point;

        public uint Point
        {
            get => _point;
            set
            {
                if (value is > 0 and < 10)
                {
                    _point = value;
                }
                else
                {
                    _point = 0;
                }
            }
        }

    }
}
   