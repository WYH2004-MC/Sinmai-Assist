using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using MAI2.Util;
using MAI2System;
using Manager;
using MelonLoader.TinyJSON;
using Net.VO.Mai2;
using Path = System.IO.Path;

namespace SinmaiAssist.Utils;

public class User
{
    public static string ExportBackupData(long index, string fileName = null , bool sendMessage = true)
    {
        UserData userData = Singleton<UserDataManager>.Instance.GetUserData(index);
        if (userData.IsGuest())
        {
            if (sendMessage)
                GameMessageManager.SendMessage((int)index,"Guest Account\nUnable to export backup");
            return null;
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
        if (string.IsNullOrEmpty(fileName))
        {
            fileName = $"User{userData.Detail.UserID}-{timestamp}.json";
        }
        File.WriteAllText(Path.Combine(path, fileName), userDataJson);
        if (sendMessage)
            GameMessageManager.SendMessage((int)index,$"Export Backup Data:\n{fileName}");
        return fileName;
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

    public static string GetUserIdString(long index)
    {
        // 通过方法调用来获取常量值
        var guestUserIdBase = GetGuestUserIDBase();
        var userId = Singleton<UserDataManager>.Instance.GetUserData(index).Detail.UserID;
        return userId == guestUserIdBase ? "Guest" : userId.ToString();
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static ulong GetGuestUserIDBase()
    {
        return ConstParameter.GuestUserIDBase;
    }
}