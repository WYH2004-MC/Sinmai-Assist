using HarmonyLib;
using Process;
using System;
using System.Reflection;
using MAI2.Util;
using Manager.Achieve;
using MelonLoader;
using Util;

namespace Common
{
    public class QuickBoot
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PowerOnProcess), "OnUpdate")]
        public static bool OnPowerOnUpdate(PowerOnProcess __instance)
        {
            MelonLogger.Msg("QuickBoot: PowerOnProcess");
            
            FieldInfo _waitTime = AccessTools.Field(typeof(PowerOnProcess), "_waitTime");
            _waitTime.SetValue(__instance, 0f);
            FieldInfo _state = AccessTools.Field(typeof(PowerOnProcess), "_state");
            if (Convert.ToInt32(_state.GetValue(__instance)) == 2)
            {
                _state.SetValue(__instance, Convert.ToByte(3));
            }
            return true;
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartupProcess), "OnUpdate")]
        public static bool OnStartupUpdate(PowerOnProcess __instance)
        {
            MelonLogger.Msg("QuickBoot: StartupProcess");
            
            FieldInfo _state = AccessTools.Field(typeof(StartupProcess), "_state");
            if (Convert.ToInt32(_state.GetValue(__instance)) == 3)
            {
                FieldInfo _statusSubMsg = AccessTools.Field(typeof(StartupProcess), "_statusSubMsg");
                string[] statusSubMsg = (string[])_statusSubMsg.GetValue(__instance);
                statusSubMsg[6] = "Skip";
                statusSubMsg[7] = "Skip";
                statusSubMsg[8] = "Skip";
                statusSubMsg[9] = "Skip";
                statusSubMsg[10] = "Skip";
                _statusSubMsg.SetValue(__instance, statusSubMsg);
                Singleton<CollectionAchieve>.Instance.Configure();
                Singleton<MapMaster>.Instance.Initialize();
                _state.SetValue(__instance, Convert.ToByte(8));
                return false;
            }
            return true;
        }
    }
}