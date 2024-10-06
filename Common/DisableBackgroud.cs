using System;
using HarmonyLib;
using Monitor;
using UnityEngine;

namespace SinmaiAssist.Common;

public class DisableBackground
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(MonitorBackgroundTownController), "SetMainDisp")]
    public static bool SetMainDisp(MonitorBackgroundTownController __instance, ref bool dispflag)
    {
        dispflag = false;
        DisableSubBackground();
        return true;
    }

    private static void DisableSubBackground()
    {
        try
        {
            if (SinmaiAssist.config.Common.SinglePlayer.HideSubMonitor) return;
            GameObject leftSubMonitor = GameObject.Find("LeftMonitor")
                .transform.Find("CommonProcess(Clone)")
                .transform.Find("RearCanvas")
                .transform.Find("Sub")
                .Find("UI_SubBackground").gameObject;
            leftSubMonitor.SetActive(false);
            GameObject rightSubMonitor = GameObject.Find("RightMonitor")
                .transform.Find("CommonProcess(Clone)")
                .transform.Find("RearCanvas")
                .transform.Find("Sub")
                .Find("UI_SubBackground").gameObject;
            rightSubMonitor.SetActive(false);
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}