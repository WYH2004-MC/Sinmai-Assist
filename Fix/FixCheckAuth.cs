using AMDaemon.Allnet;
using HarmonyLib;
using Manager;
using Manager.Operation;

namespace SinmaiAssist.Fix;

public class FixCheckAuth
{
    // codes from AquaMai [https://github.com/MewoLab/AquaMai/blob/main/AquaMai.Mods/Fix/FixCheckAuth.cs]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(OperationManager), "CheckAuth_Proc")]
    private static void PostCheckAuthProc(ref OperationData ____operationData)
    {
        if (Auth.GameServerUri.StartsWith("http://") || Auth.GameServerUri.StartsWith("https://"))
        {
            ____operationData.ServerUri = Auth.GameServerUri;
        }
    }
}