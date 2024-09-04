using HarmonyLib;

namespace Common
{
    public class CustomVersionText
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MAI2System.Config), "get_displayVersionString")]
        public static bool CustomVersionString(ref string __result)
        {
            if (SinmaiAssist.SinmaiAssist.config.CustomVersionText == null) return true;

            __result = SinmaiAssist.SinmaiAssist.config.CustomVersionText;
            return false;
        }
    }
}
