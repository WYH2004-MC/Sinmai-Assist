using static Manager.CameraManager;
using System.Collections;
using HarmonyLib;
using Manager;
using UnityEngine;

namespace SinmaiAssist.SDGB;

public class CustomCameraId
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraManager), "CameraInitialize")]
    public static bool CameraInitialize(CameraManager __instance, ref IEnumerator __result)
    {
        __result = CameraInitialize(__instance);
        return false;
    }

    public static IEnumerator CameraInitialize(CameraManager __instance)
    {
        var webcamtexField = AccessTools.Field(typeof(CameraManager), "_webcamtex");
        WebCamTexture[] Webcamtex = new WebCamTexture[2];
        int CustomQrCameraId = SinmaiAssist.config.China.CustomCameraId.QrCameraId;
        int CustomPhotoCameraId = SinmaiAssist.config.China.CustomCameraId.PhotoCameraId;
        int QrCameraId = ((CustomQrCameraId < WebCamTexture.devices.Length) ? CustomQrCameraId : 0);
        int PhotoCameraId = ((CustomPhotoCameraId < WebCamTexture.devices.Length) ? CustomPhotoCameraId : 0);
        Webcamtex[QrCameraId] = new WebCamTexture(WebCamTexture.devices[QrCameraId].name, QrCameraParam.Width, QrCameraParam.Height, QrCameraParam.Fps);
        Webcamtex[PhotoCameraId] = new WebCamTexture(WebCamTexture.devices[PhotoCameraId].name, GameCameraParam.Width, GameCameraParam.Height, GameCameraParam.Fps);
        webcamtexField.SetValue(__instance, Webcamtex);
        DeviceId[0] = QrCameraId;
        DeviceId[1] = PhotoCameraId;
        __instance.isAvailableCamera[0] = true;
        __instance.isAvailableCamera[1] = true;
        __instance.cameraProcMode[0] = CameraProcEnum.Good;
        __instance.cameraProcMode[1] = CameraProcEnum.Good;

        CameraManager.IsReady = true;
        yield break;
    }
}