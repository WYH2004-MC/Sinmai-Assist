using HarmonyLib;
using Net;
using Net.Packet;
using System;
using System.IO;
using System.Text;
using Utils;

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

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Packet), "set_State")]
        public static void NetworkListener(Packet __instance)
        {
            HttpMessageType httpMessageType = HttpMessageType.Null;
            string content = null;
            if (__instance.State == PacketState.Ready)
            {
                content = __instance.Query.GetRequest();
                string baseUrl = (string) AccessTools.Field(typeof(Packet), "BaseUrl").GetValue(__instance);
                //MelonLogger.Msg($"{baseUrl}{__instance.Query.Api} {content}");
                httpMessageType = HttpMessageType.Request;
            }
            if (__instance.State == PacketState.Done && __instance.Status == PacketStatus.Ok)
            {
                var clientField = AccessTools.Field(typeof(Packet), "Client");
                NetHttpClient client = (NetHttpClient)clientField.GetValue(__instance);
                byte[] encryptData = client.GetResponse().ToArray();
                byte[] data = CipherAES.Decrypt(encryptData);
                content = Encoding.UTF8.GetString(data);
                //MelonLogger.Msg($"{__instance.Query.Api} Response: {content}");
                httpMessageType = HttpMessageType.Response;
            }
            PrintNetworkLog(httpMessageType, __instance.Query, content);
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
