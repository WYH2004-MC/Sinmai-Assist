using HarmonyLib;
using Manager;
using static Manager.InputManager;

namespace Common
{
    public class ForwardATouchRegionToButton
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Manager.InputManager), "GetButtonDown")]
        public static void GetButtonDown(ref bool __result, int monitorId, ButtonSetting button)
        {
            if (__result) return;
            if (button.ToString().StartsWith("Button"))
            {
                __result = GetTouchPanelAreaDown(monitorId, (TouchPanelArea)button);
            }
            else if (button.ToString().Equals("Select"))
            {
                __result = GetTouchPanelAreaLongPush(monitorId, TouchPanelArea.C1, 500L) || GetTouchPanelAreaLongPush(monitorId, TouchPanelArea.C2, 500L);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Manager.InputManager), "GetButtonPush")]
        public static void GetButtonPush(ref bool __result, int monitorId, ButtonSetting button)
        {
            if (__result) return;
            if (button.ToString().StartsWith("Button")) __result = GetTouchPanelAreaPush(monitorId, (TouchPanelArea)button);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Manager.InputManager), "GetButtonLongPush")]
        public static void GetButtonLongPush(ref bool __result, int monitorId, ButtonSetting button, long msec)
        {
            if (__result) return;
            if(button.ToString().StartsWith("Button")) __result = GetTouchPanelAreaLongPush(monitorId, (TouchPanelArea)button, msec);
        }
    }
}
