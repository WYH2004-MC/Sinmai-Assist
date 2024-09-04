using HarmonyLib;
using Net.Packet;
using Net.Packet.Mai2;
using Net.VO;
using Net.VO.Mai2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Cheat
{
    public class UnlockEvent
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PacketGetGameEvent), "Proc")]
        public static void Unlock(PacketGetGameEvent __instance, ref PacketState __result)
        {
            if (__result != PacketState.Done) return;
            NetQuery<GameEventRequestVO, GameEventResponseVO> netQuery = __instance.Query as NetQuery<GameEventRequestVO, GameEventResponseVO>;
            List<GameEvent> list = new List<GameEvent>();
            foreach (DirectoryInfo optFolder in new DirectoryInfo("./Sinmai_Data/StreamingAssets").EnumerateDirectories("*", SearchOption.AllDirectories))
            {
                if (optFolder.Name.StartsWith("A") && int.TryParse(optFolder.Name.Replace("A", ""), out var optNumber))
                {
                    try
                    {
                        foreach (DirectoryInfo eventFolder in new DirectoryInfo(
                                         $"./Sinmai_Data/StreamingAssets/{optFolder.Name}/event")
                                     .EnumerateDirectories("*", SearchOption.AllDirectories))
                        {
                            try
                            {
                                if (eventFolder.Name.StartsWith("event") &&
                                    int.TryParse(eventFolder.Name.Replace("event", ""), out var eventId))
                                {
                                    var item = default(GameEvent);
                                    item.id = eventId;
                                    item.startDate = "2000-01-01 00:00:00";
                                    item.endDate = "2077-07-21 11:45:14";
                                    item.type = 1;
                                    list.Add(item);
                                }
                            }
                            catch (Exception e)
                            {
                                continue;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }
            }
            
            netQuery.Response.gameEventList = list.ToArray();
            FieldInfo onDoneField = typeof(PacketGetGameEvent).GetField("_onDone", BindingFlags.NonPublic | BindingFlags.Instance);
            Action<GameEvent[]> onDone = (Action<GameEvent[]>)onDoneField.GetValue(__instance);
            onDone?.Invoke(netQuery.Response.gameEventList ?? Array.Empty<GameEvent>());
        }
    }
}
