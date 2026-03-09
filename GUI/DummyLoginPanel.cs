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
    }
}
