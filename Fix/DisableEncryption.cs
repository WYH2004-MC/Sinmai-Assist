using HarmonyLib;
using Net;
using Net.Packet;
using System.Collections.Generic;
using System.Reflection;


namespace SinmaiAssist.Fix;

public class DisableEncryption
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(NetHttpClient), "CheckServerHash")]
    public static bool CheckServerHash(ref bool __result)
    {
        __result = true;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Packet), "Obfuscator")]
    private static bool PreObfuscator(string srcStr, ref string __result)
    {
        __result = srcStr.Replace("MaimaiExp", "").Replace("MaimaiChn", "");
        return false;
    }

    [HarmonyTargetMethods]
    public static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method("Net.CipherAES:Encrypt");
        yield return AccessTools.Method("Net.CipherAES:Decrypt");
    }

    [HarmonyPrefix]
    public static bool Encrypt(byte[] data, ref byte[] __result)
    {
        __result = data;
        return false;
    }

    [HarmonyPrefix]
    public static bool Decrypt(byte[] encryptData, ref byte[] __result)
    {
        __result = encryptData;
        return false;
    }
}