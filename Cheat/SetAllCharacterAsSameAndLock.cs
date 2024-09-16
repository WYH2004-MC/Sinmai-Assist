using System;
using System.Reflection;
using HarmonyLib;
using Net.Packet;
using Net.Packet.Mai2;
using Net.VO;
using Net.VO.Mai2;

namespace Cheat
{
    public class SetAllCharacterAsSameAndLock
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PacketGetUserData), "SafeNullMember")]
        public static bool SafeNullMember(PacketGetUserData __instance, UserDetailResponseVO src)
        {
            if (src.userData.charaSlot != null)
            {
                var newSlot = new[]
                {
                    src.userData.charaSlot[0], src.userData.charaSlot[0], src.userData.charaSlot[0],
                    src.userData.charaSlot[0], src.userData.charaSlot[0]
                };
                src.userData.charaSlot = newSlot;
                src.userData.charaLockSlot = newSlot;
            }
            return true;
        }
    }
}