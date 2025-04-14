using HarmonyLib;
using Main;
using Manager;
using MelonLoader;
using System;
using System.Reflection;
using UnityEngine;

namespace SinmaiAssist.Common;

public class SinglePlayer
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameMainObject), "Start")]
    public static bool TurnOffRightMonitor(GameMainObject __instance)
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            return false;
        }

        var position = mainCamera.gameObject.transform.position;
        if (SinmaiAssist.MainConfig.Common.SinglePlayer.HideSubMonitor)
        {
            mainCamera.gameObject.transform.position = new Vector3(-540f, -420f, position.z);
            mainCamera.orthographicSize = 540f;
        }
        else
        {
            mainCamera.gameObject.transform.position = new Vector3(-540f, position.y, position.z);
        }
        var rightMonitorField = typeof(GameMainObject).GetField("rightMonitor", BindingFlags.NonPublic | BindingFlags.Instance);
        Transform rightMonitor = (Transform)rightMonitorField.GetValue(__instance);
        rightMonitor.transform.localScale = Vector3.zero;

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
            if (SinmaiAssist.MainConfig.Common.SinglePlayer.HideSubMonitor)
            {
                vertexArray[i] = RectTransformUtility.WorldToScreenPoint(
                    Camera.main,
                    new Vector2(
                        (__instance.transform.position.x + customGraphic.vertex[i].x + 540f) * Camera.main.orthographicSize / 540f,
                        (__instance.transform.position.y + customGraphic.vertex[i].y + 420f) * Camera.main.orthographicSize / 540f
                    )
                );
            }
            else
            {
                vertexArray[i] = RectTransformUtility.WorldToScreenPoint(
                    Camera.main,
                    new Vector2(
                        __instance.transform.position.x + customGraphic.vertex[i].x + 540f,
                        __instance.transform.position.y + customGraphic.vertex[i].y
                    )
                );
            }
        }
        vertexArrayField.SetValue(__instance, vertexArray);
        return false;
    }
}