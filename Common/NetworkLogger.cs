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
        public static IntPtr NetConsole;
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
            try
            {
                if (__instance.State == PacketState.Ready)
                {
                    string content = __instance.Query.GetRequest();
                    string baseUrl = (string)AccessTools.Field(typeof(Packet), "BaseUrl").GetValue(__instance);
                    PrintNetworkLog(HttpMessageType.Request, __instance.Query, content);
                }
            }
            catch (Exception e)
            {
                MelonLogger.Error(e);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Encoding), "GetString", new[] { typeof(byte[]) })]
        public static void ResponseListener(Packet __instance, ref string __result)
        {
            try
            {
                if (isInsideProcImpl && __instance.State == PacketState.Process)
                {
                    string ccontent = __result;
                    PrintNetworkLog(HttpMessageType.Response, __instance.Query, ccontent);
                }
            }
            catch (Exception e)
            {
                MelonLogger.Error(e);
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
        private static void PrintNetworkLog(HttpMessageType httpMessageType, INetQuery query, string content)
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

            if (SinmaiAssist.SinmaiAssist.config.NetworkLoggerPrintToConsole) 
                MelonLogger.Msg($"[NetworkLogger] [{httpMessageType}] [{query.Api}] -> {content}");
        }
    }
}
