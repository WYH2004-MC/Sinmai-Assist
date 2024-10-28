using HarmonyLib;
using Manager;
using SinmaiAssist.Utils;

namespace SinmaiAssist.Cheat;

public class UnlockMusic
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(NotesListManager), "IsUnlockBase")]
    public static void UnlockBase(ref bool __result,ref int id, ref int index)
    {
        if (__result == false && SinmaiAssist.config.Cheat.SaveUnlockMusic)
        {
            User.GetUserData(index).AddUnlockMusic(UserData.MusicUnlock.Base, id);
        }
        __result = true;
    }
}