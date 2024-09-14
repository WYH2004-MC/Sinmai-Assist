using UnityEngine;

namespace Common
{
    public class Graphic
    {
        public static void ToggleFullscreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }

        public static int GetResolutionWidth()
        {
            return Screen.width;
        } 
        public static int GetResolutionHeight()
        {
            return Screen.height;
        }

        public static void SetResolution(int width, int height)
        {
            Screen.SetResolution(width, height, Screen.fullScreen);
        }

        public static int GetMaxFrameRate()
        {
            return Application.targetFrameRate;
        }

        public static void SetMaxFrameRate(int fps)
        {
            Application.targetFrameRate = fps;
            QualitySettings.vSyncCount = 0;
        }
    }
}