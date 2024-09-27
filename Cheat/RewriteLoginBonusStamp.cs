using System;
using System.Reflection;
using HarmonyLib;
using Net.Packet;
using Net.Packet.Mai2;
using Net.VO;
using Net.VO.Mai2;

namespace SinmaiAssist.Cheat;

public class RewriteLoginBonusStamp
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PacketGetUserLoginBonus), "SafeNullMember")]
    public static bool SafeNullMember(PacketGetUserData __instance, UserLoginBonusResponseVO src)
    {
        if (src.userLoginBonusList == null) return true;
        for (int i = 0; i < src.userLoginBonusList.Length; i++)
        {
            src.userLoginBonusList[i].point = SinmaiAssist.config.Cheat.RewriteLoginBonusStamp.Point;
        }
        return false;
    }
}