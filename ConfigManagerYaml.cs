   using System;
   using System.IO;
   using YamlDotNet.Serialization;
   using YamlDotNet.Serialization.NamingConventions;

   namespace SinmaiAssist
   {
       public class ConfigManagerYaml
       {
           public bool DummyLogin { get; private set; }
           public string DefaultDummyUserId { get; private set; }
           public bool CustomCameraId { get; private set; }
           public int CustomQrCameraId { get; private set; }
           public int CustomPhotoCameraId { get; private set; }
           public bool DisableMask { get; private set; }
           public bool ShowFPS { get; private set; }
           public bool NetworkLogger { get; private set; }
           public bool NetworkLoggerPrintToConsole { get; private set; }
           public bool SinglePlayer { get; private set; }
           public bool ForwardATouchRegionToButton { get; private set; }
           public string CustomVersionText { get; private set; }
           public bool SkipWarningScreen { get; private set; }
           public bool QuickBoot { get; private set; }
           public bool BlockCoin { get; private set; }
           public bool AutoPlay { get; private set; }
           public bool FastSkip { get; private set; }
           public bool ChartTimer { get; private set; }
           public bool AllCollection { get; private set; }
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
           public bool ForceIsChinaBuild { get; private set; }
           public bool SafeMode { get; private set; }

           public void Initialize()
           {
               var deserializer = new DeserializerBuilder()
                   .WithNamingConvention(CamelCaseNamingConvention.Instance)
                   .Build();

               var config = deserializer.Deserialize<Config>(File.ReadAllText($"{BuildInfo.Name}/Config.yaml"));

               DummyLogin = config.China.DummyLogin;
               DefaultDummyUserId = config.China.DefaultDummyUserId;
               CustomCameraId = config.China.CustomCameraId;
               CustomQrCameraId = config.China.CustomQrCameraId;
               CustomPhotoCameraId = config.China.CustomPhotoCameraId;

               DisableMask = config.Common.DisableMask;
               ShowFPS = config.Common.ShowFPS;
               NetworkLogger = config.Common.NetworkLogger;
               NetworkLoggerPrintToConsole = config.Common.NetworkLoggerPrintToConsole;
               SinglePlayer = config.Common.SinglePlayer;
               ForwardATouchRegionToButton = config.Common.ForwardATouchRegionToButton;
               CustomVersionText = config.Common.CustomVersionText;
               SkipWarningScreen = config.Common.SkipWarningScreen;
               QuickBoot = config.Common.QuickBoot;
               BlockCoin = config.Common.BlockCoin;

               AutoPlay = config.Cheat.AutoPlay;
               FastSkip = config.Cheat.FastSkip;
               ChartTimer = config.Cheat.ChartTimer;
               AllCollection = config.Cheat.AllCollection;
               UnlockEvent = config.Cheat.UnlockEvent;
               ResetLoginBonusRecord = config.Cheat.ResetLoginBonusRecord;
               ForceCurrentIsBest = config.Cheat.ForceCurrentIsBest;

               DisableEncryption = config.Fix.DisableEncryption;
               DisableReboot = config.Fix.DisableReboot;
               SkipVersionCheck = config.Fix.SkipVersionCheck;
               RewriteNoteJudgeSetting = config.Fix.RewriteNoteJudgeSetting;
               AdjustTiming = config.Fix.AdjustTiming;
               JudgeTiming = config.Fix.JudgeTiming;

               ShowVersionInfo = config.ModSetting.ShowVersionInfo;
               ForceIsChinaBuild = config.ModSetting.ForceIsChinaBuild;
               SafeMode = config.ModSetting.SafeMode;
           }
       }

       public class Config
       {
           public ChinaSettings China { get; set; }
           public CommonSettings Common { get; set; }
           public CheatSettings Cheat { get; set; }
           public FixSettings Fix { get; set; }
           public ModSettings ModSetting { get; set; }
       }

       public class ChinaSettings
       {
           public bool DummyLogin { get; set; }
           public string DefaultDummyUserId { get; set; }
           public bool CustomCameraId { get; set; }
           public int CustomQrCameraId { get; set; }
           public int CustomPhotoCameraId { get; set; }
       }

       public class CommonSettings
       {
           public bool DisableMask { get; set; }
           public bool ShowFPS { get; set; }
           public bool NetworkLogger { get; set; }
           public bool NetworkLoggerPrintToConsole { get; set; }
           public bool SinglePlayer { get; set; }
           public bool ForwardATouchRegionToButton { get; set; }
           public string CustomVersionText { get; set; }
           public bool SkipWarningScreen { get; set; }
           public bool QuickBoot { get; set; }
           public bool BlockCoin { get; set; }
       }

       public class CheatSettings
       {
           public bool AutoPlay { get; set; }
           public bool FastSkip { get; set; }
           public bool ChartTimer { get; set; }
           public bool AllCollection { get; set; }
           public bool UnlockEvent { get; set; }
           public bool ResetLoginBonusRecord { get; set; }
           public bool ForceCurrentIsBest { get; set; }
       }

       public class FixSettings
       {
           public bool DisableEncryption { get; set; }
           public bool DisableReboot { get; set; }
           public bool SkipVersionCheck { get; set; }
           public bool RewriteNoteJudgeSetting { get; set; }
           public float AdjustTiming { get; set; }
           public float JudgeTiming { get; set; }
       }

       public class ModSettings
       {
           public bool ShowVersionInfo { get; set; }
           public bool ForceIsChinaBuild { get; set; }
           public bool SafeMode { get; set; }
       }
   }
   