using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace SinmaiAssist.Utils;

public static class Keyboard
{

    [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
    private static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
    [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
    private static extern void keybd_event(int bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

    public static Task<int> PressKey(int bVk)
    {
        keybd_event(bVk, 0, 0, 0);
        keybd_event(bVk, 0, 2, 0);
        return Task.FromResult(0);
    }

    public static Task<int> LongPressKey(int bVk, int delay = 100)
    {
        keybd_event(bVk, 0, 0, 0);
        Thread.Sleep(delay);
        keybd_event(bVk, 0, 2, 0);
        return Task.FromResult(0);
    }

    public static Task<int> PressKey(Keys bVk)
    {
        keybd_event(bVk, 0, 0, 0);
        keybd_event(bVk, 0, 2, 0);
        return Task.FromResult(0);
    }

    public static Task<int> LongPressKey(Keys bVk, int delay = 100)
    {
        for (int i = 0; i < delay; i += 5)
        {
            keybd_event(bVk, 0, 0, 0);
            Thread.Sleep(5);
        }
        keybd_event(bVk, 0, 2, 0);
        return Task.FromResult(0);
    }

    public static Task<int> Keydown(Keys bVk)
    {
        keybd_event(bVk, 0, 0, 0);
        return Task.FromResult(0);
    }

    public static Task<int> Keydown(int bVk)
    {
        keybd_event(bVk, 0, 0, 0);
        return Task.FromResult(0);
    }

    public static Task<int> Keyup(Keys bVk)
    {
        keybd_event(bVk, 0, 2, 0);
        return Task.FromResult(0);
    }

    public static Task<int> Keyup(int bVk)
    {
        keybd_event(bVk, 0, 2, 0);
        return Task.FromResult(0);
    }
}

// Decompiled with JetBrains decompiler
// Type: System.Windows.Forms.Keys
// Assembly: System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: F521DE6A-D33E-4966-B340-B6A8A9CE7365
// Assembly location: C:\Users\Error063\RiderProjects\Sinmai-Assist\Output\System.Windows.Forms.dll

public enum Keys
{
    None = 0,
    LButton = 1,
    RButton = 2,
    Cancel = RButton | LButton, // 0x00000003
    MButton = 4,
    XButton1 = MButton | LButton, // 0x00000005
    XButton2 = MButton | RButton, // 0x00000006
    Back = 8,
    Tab = Back | LButton, // 0x00000009
    LineFeed = Back | RButton, // 0x0000000A
    Clear = Back | MButton, // 0x0000000C
    Return = Clear | LButton, // 0x0000000D
    Enter = Return, // 0x0000000D
    ShiftKey = 16, // 0x00000010
    ControlKey = ShiftKey | LButton, // 0x00000011
    Menu = ShiftKey | RButton, // 0x00000012
    Pause = Menu | LButton, // 0x00000013
    CapsLock = ShiftKey | MButton, // 0x00000014
    Capital = CapsLock, // 0x00000014
    KanaMode = Capital | LButton, // 0x00000015
    HanguelMode = KanaMode, // 0x00000015
    HangulMode = HanguelMode, // 0x00000015
    JunjaMode = HangulMode | RButton, // 0x00000017
    FinalMode = ShiftKey | Back, // 0x00000018
    KanjiMode = FinalMode | LButton, // 0x00000019
    HanjaMode = KanjiMode, // 0x00000019
    Escape = HanjaMode | RButton, // 0x0000001B
    IMEConvert = FinalMode | MButton, // 0x0000001C
    IMENonconvert = IMEConvert | LButton, // 0x0000001D
    IMEAceept = IMEConvert | RButton, // 0x0000001E
    IMEModeChange = IMEAceept | LButton, // 0x0000001F
    Space = 32, // 0x00000020
    PageUp = Space | LButton, // 0x00000021
    Prior = PageUp, // 0x00000021
    PageDown = Space | RButton, // 0x00000022
    Next = PageDown, // 0x00000022
    End = Next | LButton, // 0x00000023
    Home = Space | MButton, // 0x00000024
    Left = Home | LButton, // 0x00000025
    Up = Home | RButton, // 0x00000026
    Right = Up | LButton, // 0x00000027
    Down = Space | Back, // 0x00000028
    Select = Down | LButton, // 0x00000029
    Print = Down | RButton, // 0x0000002A
    Execute = Print | LButton, // 0x0000002B
    PrintScreen = Down | MButton, // 0x0000002C
    Snapshot = PrintScreen, // 0x0000002C
    Insert = Snapshot | LButton, // 0x0000002D
    Delete = Snapshot | RButton, // 0x0000002E
    Help = Delete | LButton, // 0x0000002F
    D0 = Space | ShiftKey, // 0x00000030
    D1 = D0 | LButton, // 0x00000031
    D2 = D0 | RButton, // 0x00000032
    D3 = D2 | LButton, // 0x00000033
    D4 = D0 | MButton, // 0x00000034
    D5 = D4 | LButton, // 0x00000035
    D6 = D4 | RButton, // 0x00000036
    D7 = D6 | LButton, // 0x00000037
    D8 = D0 | Back, // 0x00000038
    D9 = D8 | LButton, // 0x00000039
    A = 65, // 0x00000041
    B = 66, // 0x00000042
    C = B | LButton, // 0x00000043
    D = 68, // 0x00000044
    E = D | LButton, // 0x00000045
    F = D | RButton, // 0x00000046
    G = F | LButton, // 0x00000047
    H = 72, // 0x00000048
    I = H | LButton, // 0x00000049
    J = H | RButton, // 0x0000004A
    K = J | LButton, // 0x0000004B
    L = H | MButton, // 0x0000004C
    M = L | LButton, // 0x0000004D
    N = L | RButton, // 0x0000004E
    O = N | LButton, // 0x0000004F
    P = 80, // 0x00000050
    Q = P | LButton, // 0x00000051
    R = P | RButton, // 0x00000052
    S = R | LButton, // 0x00000053
    T = P | MButton, // 0x00000054
    U = T | LButton, // 0x00000055
    V = T | RButton, // 0x00000056
    W = V | LButton, // 0x00000057
    X = P | Back, // 0x00000058
    Y = X | LButton, // 0x00000059
    Z = X | RButton, // 0x0000005A
    LWin = Z | LButton, // 0x0000005B
    RWin = X | MButton, // 0x0000005C
    Apps = RWin | LButton, // 0x0000005D
    NumPad0 = 96, // 0x00000060
    NumPad1 = NumPad0 | LButton, // 0x00000061
    NumPad2 = NumPad0 | RButton, // 0x00000062
    NumPad3 = NumPad2 | LButton, // 0x00000063
    NumPad4 = NumPad0 | MButton, // 0x00000064
    NumPad5 = NumPad4 | LButton, // 0x00000065
    NumPad6 = NumPad4 | RButton, // 0x00000066
    NumPad7 = NumPad6 | LButton, // 0x00000067
    NumPad8 = NumPad0 | Back, // 0x00000068
    NumPad9 = NumPad8 | LButton, // 0x00000069
    Multiply = NumPad8 | RButton, // 0x0000006A
    Add = Multiply | LButton, // 0x0000006B
    Separator = NumPad8 | MButton, // 0x0000006C
    Subtract = Separator | LButton, // 0x0000006D
    Decimal = Separator | RButton, // 0x0000006E
    Divide = Decimal | LButton, // 0x0000006F
    F1 = NumPad0 | ShiftKey, // 0x00000070
    F2 = F1 | LButton, // 0x00000071
    F3 = F1 | RButton, // 0x00000072
    F4 = F3 | LButton, // 0x00000073
    F5 = F1 | MButton, // 0x00000074
    F6 = F5 | LButton, // 0x00000075
    F7 = F5 | RButton, // 0x00000076
    F8 = F7 | LButton, // 0x00000077
    F9 = F1 | Back, // 0x00000078
    F10 = F9 | LButton, // 0x00000079
    F11 = F9 | RButton, // 0x0000007A
    F12 = F11 | LButton, // 0x0000007B
    F13 = F9 | MButton, // 0x0000007C
    F14 = F13 | LButton, // 0x0000007D
    F15 = F13 | RButton, // 0x0000007E
    F16 = F15 | LButton, // 0x0000007F
    F17 = 128, // 0x00000080
    F18 = F17 | LButton, // 0x00000081
    F19 = F17 | RButton, // 0x00000082
    F20 = F19 | LButton, // 0x00000083
    F21 = F17 | MButton, // 0x00000084
    F22 = F21 | LButton, // 0x00000085
    F23 = F21 | RButton, // 0x00000086
    F24 = F23 | LButton, // 0x00000087
    NumLock = F17 | ShiftKey, // 0x00000090
    Scroll = NumLock | LButton, // 0x00000091
    LShiftKey = F17 | Space, // 0x000000A0
    RShiftKey = LShiftKey | LButton, // 0x000000A1
    LControlKey = LShiftKey | RButton, // 0x000000A2
    RControlKey = LControlKey | LButton, // 0x000000A3
    LMenu = LShiftKey | MButton, // 0x000000A4
    RMenu = LMenu | LButton, // 0x000000A5
    BrowserBack = LMenu | RButton, // 0x000000A6
    BrowserForward = BrowserBack | LButton, // 0x000000A7
    BrowserRefresh = LShiftKey | Back, // 0x000000A8
    BrowserStop = BrowserRefresh | LButton, // 0x000000A9
    BrowserSearch = BrowserRefresh | RButton, // 0x000000AA
    BrowserFavorites = BrowserSearch | LButton, // 0x000000AB
    BrowserHome = BrowserRefresh | MButton, // 0x000000AC
    VolumeMute = BrowserHome | LButton, // 0x000000AD
    VolumeDown = BrowserHome | RButton, // 0x000000AE
    VolumeUp = VolumeDown | LButton, // 0x000000AF
    MediaNextTrack = LShiftKey | ShiftKey, // 0x000000B0
    MediaPreviousTrack = MediaNextTrack | LButton, // 0x000000B1
    MediaStop = MediaNextTrack | RButton, // 0x000000B2
    MediaPlayPause = MediaStop | LButton, // 0x000000B3
    LaunchMail = MediaNextTrack | MButton, // 0x000000B4
    SelectMedia = LaunchMail | LButton, // 0x000000B5
    LaunchApplication1 = LaunchMail | RButton, // 0x000000B6
    LaunchApplication2 = LaunchApplication1 | LButton, // 0x000000B7
    OemSemicolon = MediaStop | Back, // 0x000000BA
    Oemplus = OemSemicolon | LButton, // 0x000000BB
    Oemcomma = LaunchMail | Back, // 0x000000BC
    OemMinus = Oemcomma | LButton, // 0x000000BD
    OemPeriod = Oemcomma | RButton, // 0x000000BE
    OemQuestion = OemPeriod | LButton, // 0x000000BF
    Oemtilde = 192, // 0x000000C0
    OemOpenBrackets = Oemtilde | Escape, // 0x000000DB
    OemPipe = Oemtilde | IMEConvert, // 0x000000DC
    OemCloseBrackets = OemPipe | LButton, // 0x000000DD
    OemQuotes = OemPipe | RButton, // 0x000000DE
    Oem8 = OemQuotes | LButton, // 0x000000DF
    OemBackslash = Oemtilde | Next, // 0x000000E2
    ProcessKey = Oemtilde | Left, // 0x000000E5
    Attn = OemBackslash | Capital, // 0x000000F6
    Crsel = Attn | LButton, // 0x000000F7
    Exsel = Oemtilde | D8, // 0x000000F8
    EraseEof = Exsel | LButton, // 0x000000F9
    Play = Exsel | RButton, // 0x000000FA
    Zoom = Play | LButton, // 0x000000FB
    NoName = Exsel | MButton, // 0x000000FC
    Pa1 = NoName | LButton, // 0x000000FD
    OemClear = NoName | RButton, // 0x000000FE
    KeyCode = 65535, // 0x0000FFFF
    Shift = 65536, // 0x00010000
    Control = 131072, // 0x00020000
    Alt = 262144, // 0x00040000
    Modifiers = -65536, // 0xFFFF0000
    IMEAccept = IMEAceept, // 0x0000001E
    Oem1 = OemSemicolon, // 0x000000BA
    Oem102 = OemBackslash, // 0x000000E2
    Oem2 = Oem1 | XButton1, // 0x000000BF
    Oem3 = Oemtilde, // 0x000000C0
    Oem4 = Oem3 | Escape, // 0x000000DB
    Oem5 = Oem3 | IMEConvert, // 0x000000DC
    Oem6 = Oem5 | LButton, // 0x000000DD
    Oem7 = Oem5 | RButton, // 0x000000DE
    Packet = Oem3 | Right, // 0x000000E7
    Sleep = IMEAccept | A, // 0x0000005F
}

