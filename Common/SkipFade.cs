using HarmonyLib;

namespace SinmaiAssist.Common;

public class SkipFade
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(FadeProcess), "OnStart")]
    public static bool OnStart(FadeProcess __instance)
    {
        __instance.ProcessingProcess();
        return false;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(FadeProcess), "StartFadeIn")]
    public static bool StartFadeIn(FadeProcess __instance)
    {
        __instance.ProcessingProcess();
        return false;
    }
}