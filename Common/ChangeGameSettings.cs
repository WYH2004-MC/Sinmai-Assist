using System;
using System.Collections.Generic;
using DB;
using HarmonyLib;
using Manager;
using MelonLoader;
using Monitor.ModeSelect;
using SinmaiAssist.Utils;
using UnityEngine;

namespace SinmaiAssist.Common;

public class ChangeGameSettings
{
    private static readonly List<bool> SettingEnable = new List<bool> { false,false,false,false };
    private static readonly List<bool> UserSettingToggleState = new List<bool> { false, false, false, false };

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ModeSelectMonitor), "Initialize")]
    public static void SetSettings(ModeSelectMonitor __instance, int monIndex)
    {
        try
        {
            UserData userData = User.GetUserData(monIndex);
            InitSettings(userData);
            SetUserFlag(userData);
            AccessTools.Field(typeof(ModeSelectMonitor), "_isSettingEnable").SetValue(__instance, SettingEnable);
            AccessTools.Field(typeof(ModeSelectMonitor), "_isSettingToggleState").SetValue(__instance, UserSettingToggleState);
            var nullSetting = (List<GameObject>) AccessTools.Field(typeof(ModeSelectMonitor), "_nullSetting").GetValue(__instance);
            var animSetting = (List<Animator>) AccessTools.Field(typeof(ModeSelectMonitor), "_animSetting").GetValue(__instance);
            for (int i = 0; i < SettingEnable.Count; i++)
            {
                nullSetting[i].transform.GetChild(0).GetChild(5).gameObject.SetActive(!SettingEnable[i]);
                if (UserSettingToggleState[i])
                {
                    animSetting[i].Play("On_Loop", 0, 0f);
                    __instance._settingMiniWindow.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(value: true);
                    __instance._settingMiniWindow.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(value: false);
                }
                else
                {
                    animSetting[i].Play("Off_Loop", 0, 0f);
                    __instance._settingMiniWindow.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(value: false);
                    __instance._settingMiniWindow.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(value: true);
                }
                nullSetting[i].transform.GetChild(0).GetChild(4).GetChild(2).gameObject.SetActive(value: false);
            }
        }
        catch (Exception e)
        {
            MelonLogger.Error(e);
        }
    }

    private static void InitSettings(UserData userData)
    {
        SettingEnable[0] = SinmaiAssist.config.Common.ChangeGameSettings.CodeRead;
        SettingEnable[1] = SinmaiAssist.config.Common.ChangeGameSettings.IconPhoto;
        SettingEnable[2] = SinmaiAssist.config.Common.ChangeGameSettings.CharaSelect;
        SettingEnable[3] = SinmaiAssist.config.Common.ChangeGameSettings.UploadPhoto;
        UserSettingToggleState[0] = userData.Extend.ExtendContendBit.IsFlagOn(ExtendContentBitID.GotoCodeRead) && SettingEnable[0];
        UserSettingToggleState[1] = userData.Extend.ExtendContendBit.IsFlagOn(ExtendContentBitID.GotoIconPhotoShoot) && SettingEnable[1];
        UserSettingToggleState[2] = userData.Extend.ExtendContendBit.IsFlagOn(ExtendContentBitID.GotoCharaSelect) && SettingEnable[2];
        UserSettingToggleState[3] = userData.Extend.ExtendContendBit.IsFlagOn(ExtendContentBitID.PhotoAgree) && SettingEnable[3];
        MelonLogger.Msg("\n[ChangeGameSettings] SettingEnable:" + string.Join(",", SettingEnable) +"\n[ChangeGameSettings] UserSettingToggleState:" + string.Join(",", UserSettingToggleState));
    }

    private static void SetUserFlag(UserData userData)
    {
        if (!SettingEnable[0])
        {
            userData.Extend.ExtendContendBit.SetFlag(ExtendContentBitID.GotoCodeRead, false);
        }
        if (!SettingEnable[1])
        {
            userData.Extend.ExtendContendBit.SetFlag(ExtendContentBitID.GotoIconPhotoShoot, false);
        }
        if (!SettingEnable[2])
        {
            userData.Extend.ExtendContendBit.SetFlag(ExtendContentBitID.GotoCharaSelect, false);
        }
        if (!SettingEnable[3])
        {
            userData.Extend.ExtendContendBit.SetFlag(ExtendContentBitID.PhotoAgree, false);
        }
    }
}