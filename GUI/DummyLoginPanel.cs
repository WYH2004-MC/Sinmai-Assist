using Manager;
using Net.Packet;
using Net.Packet.Helper;
using SinmaiAssist.Common;
using SinmaiAssist.Utils;
using UnityEngine;

namespace SinmaiAssist.GUI;

public class DummyLoginPanel
{
    public static string DummyLoginCode = "";
    public static string DummyUserId = "";
    public static bool CodeLoginFlag = false;
    public static bool UserIdLoginFlag = false;
    
    public static void OnGUI()
    {
        GUILayout.Label("Code:", MainGUI.Style.Text);
        DummyLoginCode = GUILayout.TextArea(DummyLoginCode, GUILayout.Height(100f));
        if (GUILayout.Button("Code Login", MainGUI.Style.Button))
        {
            CodeLoginFlag = true;
            if(SinmaiAssist.GameID != "SDGB")DummyAimeLogin.ReadCard();
        }
        GUILayout.Label("UserID:", MainGUI.Style.Text);
        DummyUserId = GUILayout.TextField(DummyUserId, GUILayout.Height(20f));
        if (GUILayout.Button("UserId Login", MainGUI.Style.Button))
        {
            UserIdLoginFlag = true;
            if(SinmaiAssist.GameID != "SDGB")DummyAimeLogin.ReadCard("12312312312312312312", DummyLoginCode);
        }
        GUILayout.Label($"AMDaemon BootTime: {AMDaemon.Allnet.Auth.AuthTime}", MainGUI.Style.Text);
        if (SinmaiAssist.GameID == "SDGB")
        {
            if (GUILayout.Button("UserId Logout", MainGUI.Style.Button))
            {
                PacketHelper.StartPacket(new UserLogout(ulong.Parse(DummyUserId), AMDaemon.Allnet.Auth.AuthTime, "", LogoutComplete,LogoutFailed));
            }
        }
    }
    
    private static void LogoutComplete()
    {
        SoundManager.PlayVoice(Mai2.Voice_000001.Cue.VO_000012, 1);
        GameMessageManager.SendMessage(0,$"Id: {DummyUserId} Logout Complete.");
        GameMessageManager.SendMessage(1,$"Id: {DummyUserId} Logout Complete.");
    }

    private static void LogoutFailed(PacketStatus status)
    {
        SoundManager.PlaySE(Mai2.Mai2Cue.Cue.SE_ENTRY_AIME_ERROR, 1);
        GameMessageManager.SendMessage(0,$"Id: {DummyUserId} Logout Failed.");
        GameMessageManager.SendMessage(1,$"Id: {DummyUserId} Logout Failed.");
    }
}
