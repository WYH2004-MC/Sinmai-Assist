using DB;
using System;
using HarmonyLib;
using Manager;
using MelonLoader;
using Process;

namespace Utils;

public class GameMessageManager
{
    public static IGenericManager manager { get; private set; }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ProcessManager), "SetMessageManager")]
    public static void SetMessageManager(IGenericManager genericManager)
    {
        manager = genericManager;
    }

    public static void SendGameMessage(string message ,int monitorId = 0)
    {
        try
        {
            WindowParam param = new WindowParam()
            {
                hideTitle = true,
                text = message,
                replaceText = true,
                changeSize = true,
                sizeID = WindowSizeID.Middle
            };
            manager.Enqueue(monitorId, WindowMessageID.CollectionAttentionEmptyFavorite, param);
        }
        catch (Exception e)
        {
            MelonLogger.Error(e);
        }
    }
}