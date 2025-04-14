using static Manager.CameraManager;
using System.Collections;
using HarmonyLib;
using Manager;
using MelonLoader;
using UnityEngine;

namespace SinmaiAssist.Common;

public class CustomCameraId
{
    private static CameraParameter _gameCameraParam;
    private static CameraParameter _qrCameraParam;
    
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
        if (SinmaiAssist.GameID != "SDEZ")
        {
            WebCamDevice qrDevice = WebCamTexture.devices[SinmaiAssist.GameID == "SDGB" ? SinmaiAssist.MainConfig.Common.CustomCameraId.ChimeCameraId : SinmaiAssist.MainConfig.Common.CustomCameraId.LeftQrCameraId];
            WebCamTexture qrTexture = new WebCamTexture(qrDevice.name);
            qrTexture.Play();
            _qrCameraParam = new CameraParameter(qrTexture.width, qrTexture.height, (int)qrTexture.requestedFPS);
            AccessTools.Field(typeof(CameraManager), "QrCameraParam").SetValue(__instance, _qrCameraParam);
            qrTexture.Stop();
        }
        WebCamDevice gameDevice = WebCamTexture.devices[SinmaiAssist.MainConfig.Common.CustomCameraId.PhotoCameraId];
        WebCamTexture gameTexture = new WebCamTexture(gameDevice.name);
        gameTexture.Play();
        _gameCameraParam = new CameraParameter(gameTexture.width, gameTexture.height, (int)gameTexture.requestedFPS);
        AccessTools.Field(typeof(CameraManager), "GameCameraParam").SetValue(__instance, _gameCameraParam);
        gameTexture.Stop();
    }
    
    private static IEnumerator CameraInitialize(CameraManager __instance)
    {
        WebCamTexture[] webcamtex = new WebCamTexture[WebCamTexture.devices.Length];
        int leftQrCameraId = ((SinmaiAssist.MainConfig.Common.CustomCameraId.LeftQrCameraId < WebCamTexture.devices.Length)
            ? SinmaiAssist.MainConfig.Common.CustomCameraId.LeftQrCameraId
            : 0);
        int rightQrCameraId = ((SinmaiAssist.MainConfig.Common.CustomCameraId.RightQrCameraId < WebCamTexture.devices.Length)
            ? SinmaiAssist.MainConfig.Common.CustomCameraId.RightQrCameraId
            : 0);
        int photoCameraId = ((SinmaiAssist.MainConfig.Common.CustomCameraId.PhotoCameraId < WebCamTexture.devices.Length)
            ? SinmaiAssist.MainConfig.Common.CustomCameraId.PhotoCameraId
            : 0);
        int chimeQrCameraId = ((SinmaiAssist.MainConfig.Common.CustomCameraId.ChimeCameraId < WebCamTexture.devices.Length)
            ? SinmaiAssist.MainConfig.Common.CustomCameraId.ChimeCameraId
            : 0);

        switch (SinmaiAssist.GameID)
        {
            case "SDGB":
                webcamtex[chimeQrCameraId] = new WebCamTexture(WebCamTexture.devices[chimeQrCameraId].name, _qrCameraParam.Width, _qrCameraParam.Height, _qrCameraParam.Fps);
                DeviceId[0] = chimeQrCameraId;
                DeviceId[1] = photoCameraId;
                break;
            case "SDEZ":
                webcamtex[leftQrCameraId] = new WebCamTexture(WebCamTexture.devices[leftQrCameraId].name, _qrCameraParam.Width, _qrCameraParam.Height, _qrCameraParam.Fps);
                webcamtex[rightQrCameraId] = new WebCamTexture(WebCamTexture.devices[rightQrCameraId].name, _qrCameraParam.Width, _qrCameraParam.Height, _qrCameraParam.Fps);
                DeviceId[0] = leftQrCameraId;
                DeviceId[1] = rightQrCameraId;
                DeviceId[2] = photoCameraId;
                break;
            default:
                DeviceId[0] = photoCameraId;
                break;
        }
        webcamtex[photoCameraId] = new WebCamTexture(WebCamTexture.devices[photoCameraId].name, _gameCameraParam.Width, _gameCameraParam.Height, _gameCameraParam.Fps);
        AccessTools.Field(typeof(CameraManager), "_webcamtex").SetValue(__instance, webcamtex);
        
        __instance.isAvailableCamera = new bool[webcamtex.Length];
        __instance.cameraProcMode = new CameraManager.CameraProcEnum[webcamtex.Length];
        
        for (int i = 0; i < webcamtex.Length; i++)
        {
            __instance.isAvailableCamera[i] = true;
            __instance.cameraProcMode[i] = CameraManager.CameraProcEnum.Good;
        }

        CameraManager.IsReady = true;
        yield break;
    }
}