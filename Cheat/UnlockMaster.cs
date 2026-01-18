using HarmonyLib;
using MAI2.Util;
using Manager;
using SinmaiAssist.Attributes;
using SinmaiAssist.Utils;
using Type = System.Type;

namespace SinmaiAssist.Cheat;

public class UnlockMaster
{
    [EnableGameVersion(00000, 25000)]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(NotesListManager), "IsUnlockMaster")]
    public static void IsUnlockMaster(ref bool __result, ref int id, ref int index)
    {
        if (__result == false && SinmaiAssist.MainConfig.Cheat.UnlockMaster.SaveToUserData)
        {
            User.GetUserData(index).AddUnlockMusic(UserData.MusicUnlock.Master, id);
        }
        __result = true;
    }
    [EnableGameVersion(26000)]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(NotesListManager), "HasMasterScore")]
    public static void HasMasterScore(ref bool __result, ref int musicId, ref int index)
    {
        if (__result == false && SinmaiAssist.MainConfig.Cheat.UnlockMaster.SaveToUserData)
        {
            User.GetUserData(index).AddUnlockMusic(UserData.MusicUnlock.Master, musicId);
        }
        __result = true;
    }

    [EnableGameVersion(00000, 25000)]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(NotesListManager), "IsUnlockReMaster")]
    public static void IsUnlockReMaster(ref bool __result, ref int id, ref int index)
    {
        if (Singleton<UserDataManager>.Instance.GetUserData(index).IsEntry) return;
        if (__result == false && SinmaiAssist.MainConfig.Cheat.UnlockMaster.SaveToUserData)
        {
            User.GetUserData(index).AddUnlockMusic(UserData.MusicUnlock.ReMaster, id);
        }
        __result = true;
    }
    
    [EnableGameVersion(26000)]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(NotesListManager), "HasReMasterScore")]
    public static void HasReMasterScore(ref bool __result, ref int musicId, ref int index)
    {
        if (__result == false && SinmaiAssist.MainConfig.Cheat.UnlockMaster.SaveToUserData)
        {
            User.GetUserData(index).AddUnlockMusic(UserData.MusicUnlock.ReMaster, musicId);
        }
        __result = true;
    }
}