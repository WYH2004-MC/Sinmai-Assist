using HarmonyLib;
using System.Reflection;

namespace Common
{
    public class InputManager
    {
        public static bool TouchAreaDisplayButton = false;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Manager.InputManager), "UpdateAmInput")]
        public static void TouchAreaDisplay()
        {
            if (TouchAreaDisplayButton)
            {
                var isMouseTouchPanelVisibleField = typeof(Manager.InputManager).GetField("_isMouseTouchPanelVisible", BindingFlags.NonPublic | BindingFlags.Static);
                bool isMouseTouchPanelVisible = !(bool)isMouseTouchPanelVisibleField.GetValue(typeof(Manager.InputManager));
                isMouseTouchPanelVisibleField.SetValue(null, isMouseTouchPanelVisible);
                MouseTouchPanel[] mouseTouchPanel = Manager.InputManager.MouseTouchPanel;
                for (int i = 0; i < mouseTouchPanel.Length; i++)
                {
                    mouseTouchPanel[i].SetVisible(isMouseTouchPanelVisible);
                }
                TouchAreaDisplayButton = false;
            }
        }
    }
}
