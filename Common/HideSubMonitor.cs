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

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MeshButton), "Awake")]
        public static bool FixDummyTouchPanel(MeshButton __instance)
        {
            CustomGraphic customGraphic = __instance.targetGraphic as CustomGraphic;
            FieldInfo touchAreaField = AccessTools.Field(typeof(MeshButton), "touchArea");
            FieldInfo vertexArrayField = AccessTools.Field(typeof(MeshButton), "vertexArray");

            touchAreaField.SetValue(__instance, (Manager.InputManager.TouchPanelArea)Enum.Parse(typeof(Manager.InputManager.TouchPanelArea), __instance.name));

            Vector2[] vertexArray = new Vector2[customGraphic.vertex.Count];
            for (int i = 0; i < customGraphic.vertex.Count; i++)
            {
                vertexArray[i] = RectTransformUtility.WorldToScreenPoint(
                    Camera.main,
                    new Vector2(
                        __instance.transform.position.x + customGraphic.vertex[i].x,
                        __instance.transform.position.y + customGraphic.vertex[i].y
                    )
                );
            }
            vertexArrayField.SetValue(__instance, vertexArray);
            return false;
        }
    }
}
