using System.Runtime.CompilerServices;
using MAI2.Util;
using MAI2System;
using Manager;

namespace SinmaiAssist.Utils;

public class GetUserId
{
    // 使用方法来获取常量值
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static ulong GetGuestUserIDBase()
    {
        return ConstParameter.GuestUserIDBase;
    }

    public static string Get(long index)
    {
        // 通过方法调用来获取常量值
        var guestUserIdBase = GetGuestUserIDBase();
        var userId = Singleton<UserDataManager>.Instance.GetUserData(index).Detail.UserID;
        return userId == guestUserIdBase ? "Guest" : userId.ToString();
    }
}
