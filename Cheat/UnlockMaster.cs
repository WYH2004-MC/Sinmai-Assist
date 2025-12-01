using HarmonyLib;
using MAI2.Util;
using Manager;
using SinmaiAssist.Utils;

namespace SinmaiAssist.Cheat;

public class UnlockMaster
{
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
}