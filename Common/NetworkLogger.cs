using HarmonyLib;
using MAI2.Util;
using Manager;
using MelonLoader;
using Net.Packet;
using Net.VO;
using System;
using System.IO;

namespace SinmaiAssist.Common;

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
            // string baseUrl = (string)AccessTools.Field(typeof(Packet), "BaseUrl").GetValue(__instance);
            var baseUrl = Singleton<OperationManager>.Instance.GetBaseUri();
            NowApi = baseUrl + __instance.Query.Api;
            PrintNetworkLog(HttpMessageType.Request, NowApi, content);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(NetQuery<VOSerializer, VOSerializer>), "SetResponse")]
    public static void ResponseListener(ref string str)
    {
        if (isInsideProcImpl)
        {
            string ccontent = str;
            PrintNetworkLog(HttpMessageType.Response, NowApi, ccontent);
        }
    }

    /* Not available
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Encoding), "GetString", new[] { typeof(byte[]) })]
    public static void ResponseListener(Packet __instance, ref string __result)
    {
        if (isInsideProcImpl)
        {
            string ccontent = __result;
            PrintNetworkLog(HttpMessageType.Response, NowApi, ccontent);
        }
    }
    */

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

            if (SinmaiAssist.config.Common.NetworkLogger.PrintToConsole)
                MelonLogger.Msg($"[NetworkLogger] [{httpMessageType}] [{api}] -> {content}");

            lock (logLock)
            {
                string logDir = Path.Combine($"{BuildInfo.Name}/NetworkLogs");
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