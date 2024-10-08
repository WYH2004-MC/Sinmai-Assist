using System;
using MAI2.Util;
using Manager;
using MelonLoader;
using SinmaiAssist.Utils;
using UnityEngine;

namespace SinmaiAssist.GUI;

public class UserDataPanel
{
    private static UserData _player1 = null;
    private static UserData _player2 = null;
    
    private enum CollectionType
    {
        Icon = UserData.Collection.Icon,
        Plate = UserData.Collection.Plate,
        Title = UserData.Collection.Title,
        Partner = UserData.Collection.Partner,
        Frame = UserData.Collection.Frame
    }
    
    private static string[] _collectionId = new string[6] { "", "", "", "", "", ""};
    
    public static void OnGUI()
    {
        GUILayout.Label($"User Info:");
        try
        {
            _player1 = Singleton<UserDataManager>.Instance.GetUserData(0);
            _player2 = Singleton<UserDataManager>.Instance.GetUserData(1);
        }
        catch (Exception e)
        {
            // ignore
        }
        GUILayout.Label($"1P: {_player1.Detail.UserName} ({_player1.Detail.UserID})");
        GUILayout.Label($"2P: {_player2.Detail.UserName} ({_player2.Detail.UserID})");
        
        GUILayout.Label("Add Collections", MainGUI.Style.Title);
        foreach (CollectionType type in Enum.GetValues(typeof(CollectionType)))
        {
            GUILayout.BeginHorizontal();
            int typeId = (int)type;
            GUILayout.Label(type.ToString(), new GUIStyle(UnityEngine.GUI.skin.label){fixedWidth = 50});
            _collectionId[typeId] = GUILayout.TextField(_collectionId[typeId]);
            if (GUILayout.Button("Add", new GUIStyle(UnityEngine.GUI.skin.button){ fixedWidth = 50}))
            {
                AddCollections(0, type, _collectionId[typeId]);
                AddCollections(1, type, _collectionId[typeId]);
            }
            GUILayout.EndHorizontal();
        }
        
        GUILayout.Label("User Data Backup", MainGUI.Style.Title);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("1P")) User.ExportBackupData(0);
        if (GUILayout.Button("2P")) User.ExportBackupData(1);
        GUILayout.EndHorizontal();
        
    }

    private static void AddCollections(long index, CollectionType type, string input)
    {
        UserData userData = Singleton<UserDataManager>.Instance.GetUserData(index);
        if (userData.IsGuest())
        {
            GameMessageManager.SendGameMessage("Guest Account\nUnable to add collections", (int)index);
            return;
        }
        try
        {
            if (int.TryParse(input, out int id))
            {
                if (userData.AddCollections((UserData.Collection)type, id))
                {
                    GameMessageManager.SendGameMessage($"Add Collections \n{type} {id}", (int)index);
                }
                else
                {
                    GameMessageManager.SendGameMessage($"Failed to add Collections \n{type} {id}", (int)index);
                }
            }
            else
            {
                GameMessageManager.SendGameMessage($"Invalid ID\n {input}", (int)index);
            }
        }
        catch (Exception e)
        {
            GameMessageManager.SendGameMessage($"Unknown error", (int)index);
            MelonLogger.Error(e);
        }
    }
}