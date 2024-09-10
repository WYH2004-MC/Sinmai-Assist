using HarmonyLib;
using Net;
using Net.Packet;
using System.Collections.Generic;
using System.Reflection;
using Utils;

namespace Fix
{
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
    }

    [HarmonyPatch]
    public class Encrypt
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            return new[] { AccessTools.Method("Net.CipherAES:Encrypt") };
        }

        public static bool Prefix(byte[] data, ref byte[] __result)
        {
            if (SinmaiAssist.SinmaiAssist.config.Fix.DisableEncryption && !SinmaiAssist.SinmaiAssist.config.ModSetting.SafeMode)
            {
                __result = data;
                return false;
            }
            else { return true;}
        }
    }

    [HarmonyPatch]
    public class Decrypt
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            return new[] { AccessTools.Method("Net.CipherAES:Decrypt") };
        }

        public static bool Prefix(byte[] encryptData, ref byte[] __result)
        {
            if (SinmaiAssist.SinmaiAssist.config.Fix.DisableEncryption && !SinmaiAssist.SinmaiAssist.config.ModSetting.SafeMode)
            {
                __result = encryptData;
                return false;
            }
            else { return true;}
        }
    }
}
