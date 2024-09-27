using AMDaemon;
using HarmonyLib;
using Credit = Manager.Credit;

namespace SinmaiAssist.Common;

public class BlockCoin
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Credit), "GameCostEnoughFreedom")]
    public static void GameCostEnoughFreedom(Credit __instance, ref int __result)
    {
        __result = 2;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Credit), "GameCostEnoughPlay")]
    public static void GameCostEnoughPlay(Credit __instance, ref int __result)
    {
        __result = (int)__instance.NowCoinCost;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Credit), "IsGameCostEnough")]
    public static void IsGameCostEnough(Credit __instance, ref bool __result)
    {
        __result = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Credit), "IsGameCostEnoughFreedom")]
    public static void IsGameCostEnoughFreedom(Credit __instance, ref bool __result)
    {
        __result = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Credit), "IsZero")]
    public static void IsZero(Credit __instance, ref bool __result)
    {
        __result = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Credit), "PayGameCostFreedom")]
    public static void PayGameCostFreedom(Credit __instance, ref bool __result)
    {
        __result = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Credit), "PayItemCost")]
    public static void PayItemCost(Credit __instance, ref bool __result)
    {
        __result = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Credit), "PayTicketCost")]
    public static void PayTicketCost(Credit __instance, ref bool __result)
    {
        __result = true;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CreditUnit), "get_Credit")]
        static bool PreCredit(ref uint __result)
        {
            __result = 24;
            return false;
        }
    }
}