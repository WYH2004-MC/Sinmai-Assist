using HarmonyLib;
using MelonLoader;
using Net;
using Net.Packet;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using static MelonLoader.MelonLogger;

namespace Common
{
    public class NetworkLogger
    {
        private enum HttpMessageType
        {
            Request,
            Response,
            Null
        }
        private static bool isInsideProcImpl = false;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Packet), "set_State")]
        public static void RequestListener(Packet __instance)
        {
            
            if (__instance.State == PacketState.Ready)
            {
                string content = __instance.Query.GetRequest();
                string baseUrl = (string)AccessTools.Field(typeof(Packet), "BaseUrl").GetValue(__instance);
                PrintNetworkLog(HttpMessageType.Request, __instance.Query, content);
                //MelonLogger.Msg($"[Request] [{__instance.Query.Api}] -> {content}");
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Encoding), "GetString", new[] { typeof(byte[]) })]
        public static void ResponseListener(Packet __instance, ref string __result)
        {
            if (isInsideProcImpl)
            {
                string ccontent = __result;
                PrintNetworkLog(HttpMessageType.Response, __instance.Query, ccontent);
                //MelonLogger.Msg($"[Response] [{__instance.Query.Api}] -> {ccontent}");
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Packet), "ProcImpl")]
        public static void ProcImplPrefix()
        {
            isInsideProcImpl = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Packet), "ProcImpl")]
        public static void ProcImplPostfix()
        {
            isInsideProcImpl = false;
        }
        private static void PrintNetworkLog(HttpMessageType httpMessageType, INetQuery query , string content)
        {
            if (httpMessageType == HttpMessageType.Null) return;
            DateTime now = DateTime.Now;
            string LogText = now.ToString("[yyyy-MM-dd HH:mm:ss.fff] ");
            LogText += $"[{httpMessageType}] [{query.Api}] -> {content}\n";

            if (!Directory.Exists(Path.Combine($"{SinmaiAssist.BuildInfo.Name}/NetworkLogs")))
            {
                Directory.CreateDirectory($"{SinmaiAssist.BuildInfo.Name}/NetworkLogs");
            }
            File.AppendAllText(Path.Combine($"{SinmaiAssist.BuildInfo.Name}/NetworkLogs/" + now.ToString("yyyy-MM-dd") + ".log"), LogText);
        }
    }
}
