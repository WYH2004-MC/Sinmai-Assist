using AMDaemon;
using HarmonyLib;

namespace SinmaiAssist.Fix;

public class ForceAsServer
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Network), "get_IsLanAvailable")]
    private static bool IsLanAvailable(ref bool __result)
    {
        __result = false;
        return false;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(LanInstall), "get_IsServer")]
    private static bool IsServer(ref bool __result)
    {
        __result = true;
        return false;
    }
}