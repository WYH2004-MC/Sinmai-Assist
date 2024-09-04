using HarmonyLib;
using UnityEngine;

namespace Fix
{
    public class DummyTouchPanel
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(DebugInput), "GetKey")]
        public static bool GetKey(ref bool __result, KeyCode name)
        {
            __result = Input.GetKey(name);
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(DebugInput), "GetKeyDown")]
        public static bool GetKeyDown(ref bool __result, KeyCode name)
        {
            __result = Input.GetKeyDown(name);
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(DebugInput), "GetKeyUp")]
        public static bool GetKeyUp(ref bool __result, KeyCode name)
        {
            __result = Input.GetKeyUp(name);
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(DebugInput), "GetButton")]
        public static bool GetButton(ref bool __result, string name)
        {
            __result = Input.GetButton(name);
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(DebugInput), "GetButtonDown")]
        public static bool GetButtonDown(ref bool __result, string name)
        {
            __result = Input.GetButtonDown(name);
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(DebugInput), "GetButtonUp")]
        public static bool GetButtonUp(ref bool __result, string name)
        {
            __result = Input.GetButton(name);
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(DebugInput), "GetMouseButton")]
        public static bool GetMouseButton(ref bool __result, int button)
        {
            __result = Input.GetMouseButton(button);
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(DebugInput), "GetMouseButtonDown")]
        public static bool GetMouseButtonDown(ref bool __result, int button)
        {
            __result = Input.GetMouseButtonDown(button);
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(DebugInput), "GetMouseButtonUp")]
        public static bool GetMouseButtonUp(ref bool __result, int button)
        {
            __result = Input.GetMouseButtonUp(button);
            return false;
        }
    }
}
