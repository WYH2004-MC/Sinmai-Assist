using System;
using HarmonyLib;
using Net.Packet;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AMDaemon.Allnet;
using Main;
using Manager;
using Manager.Operation;
using MelonLoader;
using Net;
using Net.VO;

namespace SinmaiAssist.Fix;

public class DisableEncryption
{
    // codes from AquaMai [https://github.com/MewoLab/AquaMai/blob/main/AquaMai.Core]
    private static string apiSuffix = "";
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameMain), "LateInitialize")]
    public static void GetApiSuffix()
    {
        try
        {
            var baseNetQueryConstructor = typeof(NetQuery<VOSerializer, VOSerializer>)
                .GetConstructors()
                .First();
            apiSuffix = ((INetQuery)baseNetQueryConstructor.Invoke(
            [.. baseNetQueryConstructor
                .GetParameters()
                .Select((parameter, i) => i == 0 ? "" : parameter.DefaultValue)])).Api;
            MelonLogger.Msg($"API suffix: {apiSuffix}");
        }
        catch (Exception e)
        {
            MelonLogger.Error($"Failed to resolve the API suffix: {e}");
            apiSuffix = null;
        }
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Packet), "Obfuscator", typeof(string))]
    public static bool PreObfuscator(string srcStr, ref string __result)
    {
        if (string.IsNullOrEmpty(apiSuffix)) return false;
        if (srcStr.EndsWith(apiSuffix))
        {
            __result = srcStr.Substring(0, srcStr.Length - apiSuffix.Length);
        }
        // __result = srcStr.Replace("MaimaiExp", "").Replace("MaimaiChn", "");
        return false;
    }
    
    [HarmonyPatch]
    public class EncryptDecrypt
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            var methods = AccessTools.TypeByName("Net.CipherAES").GetMethods();
            return
            [
                methods.FirstOrDefault(it => it.Name == "Encrypt" && it.IsPublic),
                methods.FirstOrDefault(it => it.Name == "Decrypt" && it.IsPublic)
            ];
        }
    
        public static bool Prefix(object[] __args, ref object __result)
        {
            if (!SinmaiAssist.MainConfig.Fix.DisableEncryption) return true;
            if (__args.Length == 1)
            {
                // public static byte[] Encrypt(byte[] data)
                // public static byte[] Decrypt(byte[] encryptData)
                __result = __args[0];
            }
            else if (__args.Length == 2)
            {
                // public static bool Encrypt(byte[] data, out byte[] encryptData)
                // public static bool Decrypt(byte[] encryptData, out byte[] plainData)
                __args[1] = __args[0];
                __result = true;
            }
            return false;
        }
    }
}