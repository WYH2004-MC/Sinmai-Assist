using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using HarmonyLib;
using Manager.UserDatas;
using MelonLoader;
using Process;

namespace Cheat
{
    public class ForceCurrentIsBest
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(List<UserScore>), "Find")]
        public static bool RewriteFind(List<UserScore> __instance, ref UserScore __result)
        {
            // MelonLogger.Msg("ForceCurrentIsBest");
            // // 获取调用堆栈信息
            // StackTrace stackTrace = new StackTrace();
            //
            // // 获取调用当前方法的方法
            // StackFrame callerFrame = stackTrace.GetFrame(1);
            // MethodBase callerMethod = callerFrame.GetMethod();
            // MelonLogger.Msg("Caller Method: " + callerMethod.Name);
            // MelonLogger.Msg("Caller Method FullName: " + callerMethod.DeclaringType?.FullName);
            // // __result = default(UserScore);
            return true;
        }
    }
}