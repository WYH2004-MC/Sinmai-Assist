using System.Runtime.CompilerServices;
using HarmonyLib;
using Net.VO.Mai2;
using GameUserDetail = Manager.UserDatas.UserDetail;

namespace SinmaiAssist.Cheat;

/// <summary>
/// Locks the regional-map leftover distance buffer (UserDetail.MapStock) to a fixed value.
/// - Enable: the game always reads the locked value (display + gameplay consumption).
/// - SaveToUserData: uploads also carry the locked value; otherwise the real
///   value tracked from the game's own writes is restored on upload.
/// </summary>
public class LockMapStock
{
    // Real (unlocked) MapStock per user, recorded from the game's own writes,
    // so it can be restored when uploading without SaveToUserData.
    private static readonly ConditionalWeakTable<GameUserDetail, StrongBox<int>> RealValues = new();

    private static bool Enabled => SinmaiAssist.MainConfig.Cheat.LockMapStock.Enable;

    private static int LockValue => SinmaiAssist.MainConfig.Cheat.LockMapStock.LockValue;

    // Record every real write performed by the game (download, map result, debug).
    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameUserDetail), "set_MapStock")]
    public static void TrackRealWrite(GameUserDetail __instance, int value)
    {
        if (!Enabled) return;
        RealValues.GetOrCreateValue(__instance).Value = value;
    }

    // Local lock: every read (RegionalSelect/MusicSelect/PassTicket display,
    // MapResultMeter consumption, VO export) sees the locked value.
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameUserDetail), "get_MapStock")]
    public static void LockReads(ref int __result)
    {
        if (Enabled) __result = LockValue;
    }

    // Upload path (UpsertUserAll): Export() already picked up the locked value
    // through the patched getter. Restore the real value unless SaveToUserData.
    [HarmonyPostfix]
    [HarmonyPatch(typeof(VOExtensions), "Export", new[] { typeof(GameUserDetail) })]
    public static void RestoreOnUpload(GameUserDetail src, UserDetail __result)
    {
        if (!Enabled || SinmaiAssist.MainConfig.Cheat.LockMapStock.SaveToUserData) return;
        if (RealValues.TryGetValue(src, out var real))
            __result.mapStock = real.Value;
    }
}
