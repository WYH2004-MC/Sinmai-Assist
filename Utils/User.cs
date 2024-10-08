using System;
using System.IO;
using System.Linq;
using MAI2.Util;
using Manager;
using MelonLoader.TinyJSON;
using Net.VO.Mai2;

namespace SinmaiAssist.Utils;

public class User
{
    public static void ExportBackupData(long index)
    {
        UserData userData = Singleton<UserDataManager>.Instance.GetUserData(index);
        if (userData.IsGuest())
        {
            GameMessageManager.SendGameMessage("Guest Account\nUnable to export backup", (int)index);
            return;
        }
        string path = $"{BuildInfo.Name}/UserBackup";
        string userDataJson = "{";
        userDataJson += $"\"accessCode\": \"{userData.Detail.AccessCode}\",";
        userDataJson += $"\"userId\": {userData.Detail.UserID},";
        userDataJson += $"\"userName\": \"{userData.Detail.UserName}\",";
        userDataJson += $"\"rating\": {userData.Detail.Rating}";
        
        userDataJson += ",\"userFavoriteList\": " + GetFavoriteList(userData);
        userDataJson += ",\"userCharacterList\": " + GetCharacterList(userData);
        userDataJson += ",\"userMusicDetailList\": " + GetMusicDetailList(userData);
        //userDataJson += ",\"userChargeList\": " + GetChargeList(userData);
        //userDataJson += ",\"userCourseList\": " + GetCourseList(userData);

        userDataJson += "}";
        
        string timestamp = DateTime.Now.ToString("yyMMddHHmmss");
        if (!Directory.Exists(Path.Combine(path)))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllText(Path.Combine(path, $"User{userData.Detail.UserID}-{timestamp}.json"), userDataJson);
        GameMessageManager.SendGameMessage($"Export Backup Data:\nUser{userData.Detail.UserID}-{timestamp}.json", (int)index);
    }

    public static string GetCharacterList(UserData userData)
    {
        return JSON.Dump(userData.CharaList.Export(), EncodeOptions.NoTypeHints);
    }

    public static string GetChargeList(UserData userData)
    {
        return JSON.Dump(userData.ChargeList, EncodeOptions.NoTypeHints);
    } 
    
    public static string GetCourseList(UserData userData)
    {
        return JSON.Dump(userData.CourseList.Export(), EncodeOptions.NoTypeHints);
    }

    public static string GetMusicDetailList(UserData userData)
    {
        string list = "[";
        UserMusicDetail[] userMusicDetail = userData.ExportScoreDetailList();
        list += string.Join(", ", userMusicDetail.Select(detail =>
            $"{{\"musicId\": {detail.musicId}, " +
            $"\"level\": {(int)detail.level}, " +
            $"\"playCount\": {detail.playCount}, " +
            $"\"achievement\": {detail.achievement}, " +
            $"\"comboStatus\": {(int)detail.comboStatus}, " +
            $"\"syncStatus\": {(int)detail.syncStatus}, " +
            $"\"deluxscoreMax\": {detail.deluxscoreMax}, " +
            $"\"scoreRank\": {(int)detail.scoreRank}, " +
            $"\"extNum1\": {detail.extNum1}}}"
        ));
        list += "]";
        return list;
    }

    public static string GetFavoriteList(UserData userData)
    {
        string list = "[{";
        list += "\"iconList\": [" + string.Join(", ", userData.IconList.Select(item => item.itemId)) + "]";
        list += ",\"plateList\": [" + string.Join(", ", userData.PlateList.Select(item => item.itemId)) + "]";
        list += ",\"titleList\": [" + string.Join(", ", userData.TitleList.Select(item => item.itemId)) + "]";
        list += ",\"frameList\": [" + string.Join(", ", userData.FrameList.Select(item => item.itemId)) + "]";
        list += "}]";
        return list;
    }
    
    public static UserData GetUserData(long index)
    {
        return Singleton<UserDataManager>.Instance.GetUserData(index);
    }
}