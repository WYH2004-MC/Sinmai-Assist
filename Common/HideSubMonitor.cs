using HarmonyLib;
using Main;
using Manager;
using System;
using System.Reflection;
using UnityEngine;

namespace Common
{
    public class HideSubMonitor
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameMainObject), "Start")]
        public static bool TurnOffSubMonitor(GameMainObject __instance)
        {
            var mainCamera = Camera.main;

            if (mainCamera == null)
            {
                return false;
            }

            var position = mainCamera.gameObject.transform.position;
            mainCamera.gameObject.transform.position = new Vector3(position.x, -420f, position.z);
            mainCamera.orthographicSize = 540f;

            return false;
        }
    }
}
