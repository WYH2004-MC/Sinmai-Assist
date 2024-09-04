using System;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace Common
{
    public class DisableMask
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Main.GameMain), "LateInitialize", new System.Type[] { typeof(MonoBehaviour), typeof(Transform), typeof(Transform) })]
        public static bool LateInitialize(MonoBehaviour gameMainObject, ref Transform left, ref Transform right)
        {
            try
            {
                GameObject.Find("Mask").SetActive(false);
            }
            catch (Exception e)
            {
                MelonLogger.Msg("Maybe the current Sinmai build does not have GameObject \"Mask\".");
            }
            return true;
        }
    }
}
