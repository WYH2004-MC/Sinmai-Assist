using DB;
using HarmonyLib;
using MAI2System;
using Net.VO.Mai2;
using Process.Entry.State;
using SinmaiAssist.Utils;

namespace SinmaiAssist.Fix;

public class SkipVersionCheck
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ConfirmPlay), "IsValidVersion")]
    public static bool IsValidVersion(ref bool __result,ref UserPreviewResponseVO vo)
    {
        VersionNo lastRomVersion = new VersionNo();
        VersionNo lastDataVersion = new VersionNo();
        
        lastRomVersion.tryParse(vo.lastRomVersion, true);
        lastDataVersion.tryParse(vo.lastDataVersion, true);

        GameMessageManager.SendMessage(0,
                                    $"RomVersion: {lastRomVersion.versionString}\n" +
                                            $"DataVersion: {lastDataVersion.versionString}"
                                           ,"Account Version"
                                           , WindowPositionID.Upper);
        __result = true;
        return false;
    }
}