using HarmonyLib;
using Manager.UserDatas;

namespace Fix
{
    public class RewriteNoteJudgeSetting
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UserOption), "GetAdjustMSec")]
        public static void GetAdjustMSec(ref float __result)
        {
            __result = SinmaiAssist.SinmaiAssist.config.AdjustTiming;
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UserOption), "GetJudgeTimingFrame")]
        public static void GetJudgeTimingFrame(ref float __result)
        {
            __result = SinmaiAssist.SinmaiAssist.config.JudgeTiming;
        }
    }
}