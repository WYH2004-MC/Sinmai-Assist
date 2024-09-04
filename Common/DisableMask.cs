using HarmonyLib;
using UnityEngine;

namespace Common
{
    public class DisableMask
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Main.GameMain), "LateInitialize", new System.Type[] { typeof(MonoBehaviour), typeof(Transform), typeof(Transform) })]
        public static bool LateInitialize(MonoBehaviour gameMainObject, ref Transform left, ref Transform right)
        {
            GameObject.Find("Mask").SetActive(false);
            return true;
        }
    }
}
