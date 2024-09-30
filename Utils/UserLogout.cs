using System;
using AMDaemon;
using AMDaemon.Allnet;
using Manager;
using MelonLoader;
using Net.Packet;
using Net.VO;
using Net.VO.Mai2;

namespace SinmaiAssist.Utils;

public class UserLogout : Packet
{
    private readonly Action _onDone;

    private readonly Action<PacketStatus> _onError;

    private readonly ulong _userId;

    private readonly DateTime _dateTime;

    private readonly long _unixTime;

    public UserLogout(ulong userId, DateTime dateTime, string acsessCode, Action onDone, Action<PacketStatus> onError = null)
    {

        _onDone = onDone;
        _onError = onError;
        _userId = userId;
        _dateTime = dateTime;
        _unixTime = new DateTimeOffset(dateTime).ToUnixTimeSeconds();
        NetQuery<UserLogoutRequestVO, UserLogoutResponseVO> netQuery = new NetQuery<UserLogoutRequestVO, UserLogoutResponseVO>("UserLogoutApi", userId);
        
        netQuery.Request.accessCode = acsessCode;
        netQuery.Request.regionId = Auth.RegionCode;
        netQuery.Request.placeId = (int)Auth.LocationId;
        netQuery.Request.clientId = AMDaemon.System.KeychipId.ShortValue;
        netQuery.Request.dateTime = _unixTime;
        netQuery.Request.type = (int)LogoutType.Logout;
        netQuery.Request.userId = userId;
        Create(netQuery);
    }

    public override PacketState Proc()
    {
        var result = ProcImpl();
        switch (result)
        {
            case PacketState.Done:
                _onDone();
                break;
            case PacketState.Error:
                _onError?.Invoke(base.Status);
                break;
        }
        return base.State;
    }
}