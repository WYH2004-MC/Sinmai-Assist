using DB;
using System;
using HarmonyLib;
using Manager;
using MelonLoader;
using Process;

namespace SinmaiAssist.Utils;

public class GameMessageManager
{
    public static IGenericManager manager { get; private set; }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ProcessManager), "SetMessageManager")]
    public static void SetMessageManager(IGenericManager genericManager)
    {
        manager = genericManager;
    }

    public static void SendGameMessage(string message, WindowPositionID positionID = WindowPositionID.Middle)
    {
        try
        {
            WindowParam param = new WindowParam()
            {
                hideTitle = true,
                text = message,
                replaceText = true,
                changeSize = true,
                sizeID = WindowSizeID.Middle,
                positionID = positionID
            };
            manager.Enqueue(0, WindowMessageID.CollectionAttentionEmptyFavorite, positionID, param);
            manager.Enqueue(1, WindowMessageID.CollectionAttentionEmptyFavorite, positionID, param);
        }
        catch (Exception e)
        {
            MelonLogger.Error(e);
        }
    }

    public static void SendGameMessage(string message, int monitorId, WindowPositionID positionID = WindowPositionID.Middle)
    {
        try
        {
            WindowParam param = new WindowParam()
            {
                hideTitle = true,
                text = message,
                replaceText = true,
                changeSize = true,
                sizeID = WindowSizeID.Middle,
                positionID = positionID
            };
            manager.Enqueue(monitorId, WindowMessageID.CollectionAttentionEmptyFavorite, positionID, param);
        }
        catch (Exception e)
        {
            MelonLogger.Error(e);
        }
    }
}