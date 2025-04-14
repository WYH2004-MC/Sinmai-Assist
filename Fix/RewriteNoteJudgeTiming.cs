using HarmonyLib;
using Manager.UserDatas;

namespace SinmaiAssist.Fix;

public class RewriteNoteJudgeTiming
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UserOption), "GetAdjustMSec")]
    public static void GetAdjustMSec(ref float __result)
    {
        __result = SinmaiAssist.MainConfig.Fix.RewriteNoteJudgeTiming.AdjustTiming;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UserOption), "GetJudgeTimingFrame")]
    public static void GetJudgeTimingFrame(ref float __result)
    {
        __result = SinmaiAssist.MainConfig.Fix.RewriteNoteJudgeTiming.JudgeTiming;
    }
}