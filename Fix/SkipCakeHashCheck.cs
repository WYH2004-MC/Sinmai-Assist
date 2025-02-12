using HarmonyLib;
using Net;

namespace SinmaiAssist.Fix;

public class SkipCakeHashCheck
{
    // codes from AquaMai [https://github.com/MewoLab/AquaMai/blob/main/AquaMai.Mods/Fix/Common.cs]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(NetHttpClient), MethodType.Constructor)]
    private static void OnNetHttpClientConstructor(NetHttpClient __instance)
    {
        var tInstance = Traverse.Create(__instance).Field("isTrueDll");
        if (tInstance.FieldExists())
        {
            tInstance.SetValue(true);
        }
    }
}