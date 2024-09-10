using HarmonyLib;
using Manager.UserDatas;

namespace Fix
{
    public class RewriteNoteJudgeTiming
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UserOption), "GetAdjustMSec")]
        public static void GetAdjustMSec(ref float __result)
        {
            __result = SinmaiAssist.SinmaiAssist.config.Fix.RewriteNoteJudgeTiming.AdjustTiming;
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UserOption), "GetJudgeTimingFrame")]
        public static void GetJudgeTimingFrame(ref float __result)
        {
            __result = SinmaiAssist.SinmaiAssist.config.Fix.RewriteNoteJudgeTiming.JudgeTiming;
        }
    }
}