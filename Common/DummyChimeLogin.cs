using ChimeLib.NET;
using HarmonyLib;
using Manager;
using SinmaiAssist;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace SinmaiAssist.SDGB;

public class DummyChimeLogin
{
    // CameraManager Patch
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraManager), "get_IsAvailableCamera")]
    public static bool IsAvailableCamera(ref bool __result)
    {
        __result = true;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraManager), "get_IsAvailableChimeCamera")]
    public static bool IsAvailableChimeCamera(ref bool __result)
    {
        __result = true;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraManager), "get_IsAvailableCameras")]
    public static bool IsAvailableCameras(ref bool[] __result)
    {
        __result = new bool[2] { true, true };
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraManager), "GetTexture")]
    public static bool GetTexture(ref WebCamTexture __result)
    {
        __result = null;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraManager), "IsPlayingPhotoCamera")]
    public static bool IsPlayingPhotoCamera(ref bool __result)
    {
        __result = false;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraManager), "PlayPhotoCamera")]
    public static bool PlayPhotoCamera() { return false; }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraManager), "PlayPhotoOnly")]
    public static bool PlayPhotoOnly() { return false; }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraManager), "PausePhoto")]
    public static bool PausePhoto() { return false; }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraManager), "StopPhoto")]
    public static bool StopPhoto() { return false; }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraManager), "GetColor32")]
    public static bool GetColor32(ref Color32[] __result)
    {
        __result = null;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraManager), "CameraInitialize")]
    public static bool CameraInitialize(CameraManager __instance, ref IEnumerator __result)
    {
        __result = CameraInitialize(__instance);
        return false;
    }

    public static IEnumerator CameraInitialize(CameraManager __instance)
    {
        CameraManager.IsReady = true;
        yield break;
    }

    // ChimeDevice Patch
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChimeDevice), MethodType.Constructor, new[] { typeof(WebCamTexture) })]
    public static bool ChimeDevice(WebCamTexture texture) { return false; }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChimeDevice), "HasError")]
    public static bool HasError(ref bool __result)
    {
        __result = false;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChimeDevice), "IsReady")]
    public static bool IsReady(ref bool __result)
    {
        __result = true;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChimeDevice), "BeginScan")]
    public static bool BeginScan() { return false; }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChimeDevice), "EndScan")]
    public static bool EndScan() { return false; }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChimeDevice), "GetDecodeStrings")]
    public static bool GetDecodeStrings(ref string[] __result)
    {
        if (ModGUI.QrLoginFlag)
        {
            ModGUI.QrLoginFlag = false;
            if (ModGUI.DummyQrCode == null)
            {
                __result = null;
                return false;
            }
            else
            {
                __result = new string[1] { ModGUI.DummyQrCode };
                return false;
            }
        }
        __result = null;
        return false;
    }

    // ChimeReaderManager Patch
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChimeReaderManager), "Execute")]
    public static bool Execute(ChimeReaderManager __instance)
    {
        var result = AccessTools.Field(typeof(ChimeReaderManager), "_result");
        var aimeId = AccessTools.Field(typeof(ChimeReaderManager), "_aimeId");
        var currentState = AccessTools.Field(typeof(ChimeReaderManager), "currentState");
        if (ModGUI.UserIdLoginFlag)
        {
            ChimeId _aimeId;
            System.Type chimeIdType = System.Type.GetType("ChimeLib.NET.ChimeId, ChimeLib.NET");
            MethodInfo makeMethod = chimeIdType.GetMethod("Make", BindingFlags.NonPublic | BindingFlags.Static);
            _aimeId = (ChimeId)makeMethod.Invoke(null, new object[] { uint.Parse(ModGUI.DummyUserId) });
            result.SetValue(__instance, ChimeReaderManager.Result.Done);
            aimeId.SetValue(__instance, _aimeId);
            currentState.SetValue(__instance, 9);
            return false;
        }
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChimeReaderManager), "AdvCheck")]
    public static bool AdvCheck(ref bool __result)
    {
        if (ModGUI.UserIdLoginFlag)
        {
            __result = true;
            return false;
        }
        else
        {
            return true;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Process.Entry.TryAime), "Execute")]
    public static void ClearFlag()
    {
        ModGUI.UserIdLoginFlag = false;
    }
}