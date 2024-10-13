using DB;
using System;
using HarmonyLib;
using Manager;
using MelonLoader;
using Process;

namespace SinmaiAssist.Utils;

public class GameMessageManager
{
    private static IGenericManager _manager;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ProcessManager), "SetMessageManager")]
    public static void SetMessageManager(IGenericManager genericManager)
    {
        _manager = genericManager;
    }

    public static void SendMessage(int monitorId, string message, string title = null, WindowPositionID positionID = WindowPositionID.Middle, WindowMessageID messageID = WindowMessageID.CollectionAttentionEmptyFavorite)
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
            if (title != null)
            {
                if (messageID == WindowMessageID.CollectionAttentionEmptyFavorite)
                    messageID = WindowMessageID.AimeUseNotice;
                param.replaceTitle = true;
                param.hideTitle = false;
                param.title = title;
            }
            _manager.Enqueue(monitorId, messageID, positionID, param);
        }
        catch (Exception e)
        {
            MelonLogger.Error(e);
        }
    }

    public static void SendWarning(int monitorId, string message, string title, float lifeTime = 3000)
    {
        try
        {
            WarningWindowInfo warning = new WarningWindowInfo(
                title: title,
                message: message,
                lifeTime: lifeTime,
                monitorId: monitorId
            );
            _manager.EnqueueWarning(warning);
        }catch (Exception e)
        {
            MelonLogger.Error(e);
        }
    }
    
    public static IGenericManager GetMessageManager()
    {
        return _manager;
    }
}