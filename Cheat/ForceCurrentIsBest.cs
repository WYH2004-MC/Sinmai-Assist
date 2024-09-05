using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using HarmonyLib;
using Manager.UserDatas;
using MelonLoader;

namespace Cheat
{
    public class ForceCurrentIsBest
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(List<UserScore>), "Find")]
        public static bool RewriteFind(List<UserScore> __instance, ref UserScore __result)
        {
            try
            {
                MelonLogger.Msg("ForceCurrentIsBest");
                // 获取调用堆栈信息
                MelonLogger.Msg("====================================================================================");
                StackTrace stackTrace = new StackTrace();
                for (int i = 0; i < stackTrace.FrameCount; i++)
                {
                    MelonLogger.Msg("Stack Frame " + i + ":");
                    StackFrame callerFrame = stackTrace.GetFrame(8);
                    if (callerFrame == null)
                    {
                        return true;
                    }
                    MethodBase callerMethod = callerFrame.GetMethod();
                    MelonLogger.Msg("==============");
                    MelonLogger.Msg($"Caller Method: " + callerMethod.Name);
                    MelonLogger.Msg($"Caller Method FullName: " + callerMethod.DeclaringType?.FullName);
                    ParameterInfo[] parameters = callerMethod.GetParameters();
                    if (parameters.Length == 0)
                    {
                        return true;
                    }
                    foreach (var parameter in parameters)
                    {
                        MelonLogger.Msg($"Parameter Name: {parameter.Name}, Type: {parameter.ParameterType}");
                    }

                    bool fromResult = callerMethod.DeclaringType?.FullName?.Contains("Process.ResultProcess") ?? false;
                    MelonLogger.Msg($"Call from Process.ResultProcess: " + fromResult);
                    if (fromResult && callerMethod.Name.Equals("OnStart"))
                    {
                        MelonLogger.Msg("Hooking...");
                        MelonLogger.Msg("==============");
                        // __result = default(UserScore);
                        // return false;
                    }
                    MelonLogger.Msg("==============");

                }
                MelonLogger.Msg("====================================================================================");
            }
            catch (Exception e)
            {
                MelonLogger.Error("Failed");
            }
            return true;
        }
    }
}