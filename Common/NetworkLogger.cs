using HarmonyLib;
using MelonLoader;
using Net;
using Net.Packet;
using SinmaiAssist;
using System;
using System.IO;
using System.Text;

namespace Common
{
    public class NetworkLogger
    {
        private static readonly object logLock = new object();
        private enum HttpMessageType
        {
            Request,
            Response,
            Null
        }
        private static bool isInsideProcImpl = false;
        private static string NowApi = "";

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Packet), "set_State")]
        public static void RequestListener(Packet __instance)
        {
            if (__instance.State == PacketState.Ready)
            {
                string content = __instance.Query.GetRequest();
                string baseUrl = (string)AccessTools.Field(typeof(Packet), "BaseUrl").GetValue(__instance);
                NowApi = __instance.Query.Api;
                PrintNetworkLog(HttpMessageType.Request, NowApi, content);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Encoding), "GetString", new[] { typeof(byte[]) })]
        public static void ResponseListener(Packet __instance, ref string __result)
        {
            if (isInsideProcImpl && __instance.State == PacketState.Process)
            {
                string ccontent = __result;
                PrintNetworkLog(HttpMessageType.Response, NowApi, ccontent);
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
        private static void PrintNetworkLog(HttpMessageType httpMessageType, string api, string content)
        {
            if (httpMessageType == HttpMessageType.Null) return;
            try
            {
                DateTime now = DateTime.Now;
                string logText = now.ToString("[HH:mm:ss.fff] ");
                logText += $"[{httpMessageType}] [{api}] -> {content}\n";

                if (SinmaiAssist.SinmaiAssist.config.NetworkLoggerPrintToConsole)
                    MelonLogger.Msg($"[NetworkLogger] [{httpMessageType}] [{api}] -> {content}");

                lock (logLock)
                {
                    string logDir = Path.Combine($"{SinmaiAssist.BuildInfo.Name}/NetworkLogs");
                    if (!Directory.Exists(logDir))
                    {
                        Directory.CreateDirectory(logDir);
                    }
                    File.AppendAllText(Path.Combine(logDir, now.ToString("yyyy-MM-dd") + ".log"), logText);
                }
            }
            catch (Exception e)
            { 
                MelonLogger.Error(e);
            }
        }
    }
}
