using HarmonyLib;
using System;
using System.Reflection;

namespace Utils
{
    internal class CipherAES
    {
        private static Type cipherAESType = AccessTools.TypeByName("CipherAES");

        public static byte[] Encrypt(byte[] data)
        {
            MethodInfo encryptMethod = cipherAESType.GetMethod("Encrypt", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            byte[] encryptData = (byte[])encryptMethod.Invoke(null, new object[] { data });
            return encryptData;
        }

        public static byte[] Decrypt(byte[] encryptData)
        {
            MethodInfo decryptMethod = cipherAESType.GetMethod("Decrypt", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            byte[] data = (byte[])decryptMethod.Invoke(null, new object[] { encryptData });
            return data;
        }
    }
}
