using HarmonyLib;
using Process;
using System;
using System.Reflection;
using MelonLoader;

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
                _state.SetValue(__instance, Convert.ToByte(8));
            }
            return true;
        }
    }
}