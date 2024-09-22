using System;
using AMDaemon;
using AMDaemon.Allnet;
using Manager;
using MelonLoader;
using Net.Packet;
using Net.VO;
using Net.VO.Mai2;

namespace Utils;

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
        netQuery.Request.userId = userId;
        netQuery.Request.accessCode = acsessCode;
        netQuery.Request.regionId = Auth.RegionCode;
        netQuery.Request.placeId = (int)Auth.LocationId;
        netQuery.Request.clientId = AMDaemon.System.KeychipId.ShortValue;
        netQuery.Request.dateTime = _unixTime;
        netQuery.Request.type = (int)LogoutType.Logout;
        Create(netQuery);
    }

    public override PacketState Proc()
    {
        var result = ProcImpl();
        switch (result)
        {
            case PacketState.Done:
                GameMessageManager.SendGameMessage($"Id: {_userId} Logout.\nDateTime: {_dateTime} - {_unixTime}");
                SoundManager.PlayVoice(Mai2.Voice_000001.Cue.VO_000012, 1);
                _onDone();
                break;
            case PacketState.Error:
                MelonLogger.Msg($"Id: {_userId} Logout Failed\nDateTime: {_dateTime} - {_unixTime}");
                _onError?.Invoke(base.Status);
                break;
        }
        return base.State;
    }
}