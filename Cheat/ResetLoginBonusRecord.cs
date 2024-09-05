using System;
using System.Reflection;
using HarmonyLib;
using Net.Packet;
using Net.Packet.Mai2;
using Net.VO;
using Net.VO.Mai2;

namespace Cheat
{
    public class ResetLoginBonusRecord
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PacketGetUserData), "Proc")]
        public static void Proc(PacketGetUserData __instance, ref PacketState __result)
        {
            if (__result != PacketState.Done) return;
            NetQuery<UserDetailRequestVO, UserDetailResponseVO> netQuery = __instance.Query as NetQuery<UserDetailRequestVO, UserDetailResponseVO>;
            netQuery.Response.userData.lastLoginDate = "2000-01-01 00:00:00";
            netQuery.Response.userData.lastPairLoginDate = "2000-01-01 00:00:00";
            FieldInfo onDoneField = typeof(PacketGetUserData).GetField("_onDone", BindingFlags.NonPublic | BindingFlags.Instance);
            Action<UserDetail, int> onDone = (Action<UserDetail, int>)onDoneField.GetValue(__instance);
            onDone?.Invoke(netQuery.Response.userData, netQuery.Response.banState);
        }
    }
}