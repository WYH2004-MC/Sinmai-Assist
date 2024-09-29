using HarmonyLib;
using MAI2.Util;
using Manager;

namespace SinmaiAssist.Cheat;

public class UnlockMaster
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(NotesListManager), "IsUnlockMaster")]
    public static bool IsUnlockMaster(ref bool __result, ref int index)
    {
        __result = true;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(NotesListManager), "IsUnlockReMaster")]
    public static bool IsUnlockReMaster(ref bool __result, ref int index)
    {
        if (Singleton<UserDataManager>.Instance.GetUserData((long)index).IsEntry) return true;
        __result = true;
        return false;
    }


}