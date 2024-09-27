using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
