using HarmonyLib;
using MAI2.Util;
using Manager;

namespace Cheat;

public class UnlockMusic
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(NotesListManager), "IsUnlockBase")]
    public static bool UnlockBase(ref bool __result, ref int index)
    {
        __result = true;
        return false;
    }
}