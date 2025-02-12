using System.Net;
using HarmonyLib;
using Net;

namespace SinmaiAssist.Fix;

public class RestoreCertificateValidation
{
    // codes from AquaMai [https://github.com/MewoLab/AquaMai/blob/main/AquaMai.Mods/Fix/Common.cs]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(NetHttpClient), "Create")]
    private static void OnNetHttpClientCreate()
    {
        ServicePointManager.ServerCertificateValidationCallback = null;
    }
}