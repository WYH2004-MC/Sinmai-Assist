using static Manager.CameraManager;
using System.Collections;
using HarmonyLib;
using Manager;
using MelonLoader;
using UnityEngine;

namespace SinmaiAssist.Common;

public class CustomCameraId
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraManager), "CameraInitialize")]
    public static bool CameraInitialize(CameraManager __instance, ref IEnumerator __result)
    {
        __result = CameraInitialize(__instance);
        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CameraManager), "Initialize")]
    public static void SetCameraResolution(CameraManager __instance)
    {
        WebCamDevice gameDevice = WebCamTexture.devices[SinmaiAssist.config.Common.CustomCameraId.PhotoCameraId];
        WebCamDevice qrDevice = WebCamTexture.devices[SinmaiAssist.IsSDGB ? SinmaiAssist.config.Common.CustomCameraId.ChimeCameraId : SinmaiAssist.config.Common.CustomCameraId.LeftQrCameraId];
        WebCamTexture gameTexture = new WebCamTexture(gameDevice.name);
        WebCamTexture qrTexture = new WebCamTexture(qrDevice.name);
        gameTexture.Play();
        qrTexture.Play();
        CameraParameter gameCameraParam = new CameraParameter(gameTexture.width, gameTexture.height, (int)gameTexture.requestedFPS);
        CameraParameter qrCameraParam = new CameraParameter(qrTexture.width, qrTexture.height, (int)qrTexture.requestedFPS);
        AccessTools.Field(typeof(CameraManager), "GameCameraParam").SetValue(__instance, gameCameraParam);
        AccessTools.Field(typeof(CameraManager), "QrCameraParam").SetValue(__instance, qrCameraParam);
        gameTexture.Stop();
        qrTexture.Stop();
    }
    
    private static IEnumerator CameraInitialize(CameraManager __instance)
    {
        var webcamtexField = AccessTools.Field(typeof(CameraManager), "_webcamtex");
        WebCamTexture[] webcamtex = new WebCamTexture[SinmaiAssist.IsSDGB ? 2 : 3];
        int chimeCameraId = ((SinmaiAssist.config.Common.CustomCameraId.ChimeCameraId < WebCamTexture.devices.Length)
            ? SinmaiAssist.config.Common.CustomCameraId.ChimeCameraId
            : 0);
        int leftQrCameraId = ((SinmaiAssist.config.Common.CustomCameraId.LeftQrCameraId < WebCamTexture.devices.Length)
            ? SinmaiAssist.config.Common.CustomCameraId.LeftQrCameraId
            : 0);
        int rightQrCameraId = ((SinmaiAssist.config.Common.CustomCameraId.RightQrCameraId < WebCamTexture.devices.Length)
            ? SinmaiAssist.config.Common.CustomCameraId.RightQrCameraId
            : 0);
        int photoCameraId = ((SinmaiAssist.config.Common.CustomCameraId.PhotoCameraId < WebCamTexture.devices.Length)
            ? SinmaiAssist.config.Common.CustomCameraId.PhotoCameraId
            : 0);
        
        if (SinmaiAssist.IsSDGB)
        {
            webcamtex[chimeCameraId] = new WebCamTexture(WebCamTexture.devices[chimeCameraId].name, QrCameraParam.Width, QrCameraParam.Height, QrCameraParam.Fps);
            DeviceId[0] = chimeCameraId;
            DeviceId[1] = photoCameraId;
        }
        else
        {
            webcamtex[leftQrCameraId] = new WebCamTexture(WebCamTexture.devices[leftQrCameraId].name, QrCameraParam.Width, QrCameraParam.Height, QrCameraParam.Fps);
            webcamtex[rightQrCameraId] = new WebCamTexture(WebCamTexture.devices[rightQrCameraId].name, QrCameraParam.Width, QrCameraParam.Height, QrCameraParam.Fps);
            DeviceId[0] = leftQrCameraId;
            DeviceId[1] = rightQrCameraId;
            DeviceId[2] = photoCameraId;
        }
        webcamtex[photoCameraId] = new WebCamTexture(WebCamTexture.devices[photoCameraId].name, GameCameraParam.Width, GameCameraParam.Height, GameCameraParam.Fps);
        
        webcamtexField.SetValue(__instance, webcamtex);
            
        for (int i = 0; i < webcamtex.Length; i++)
        {
            __instance.isAvailableCamera[i] = true;
            __instance.cameraProcMode[i] = CameraProcEnum.Good;
        }

        CameraManager.IsReady = true;
        yield break;
    }
}