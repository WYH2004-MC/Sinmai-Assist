using UnityEngine;

namespace Common
{
    public class ShowFPS
    {
        private static string fpsText = "FPS: ";
        private static float deltaTime = 0.0f;
        
        public static void OnGUI()
        {
            Update();
            GUIStyle style = new GUIStyle();
            style.fontSize = 24;
            style.normal.textColor = Color.black;
            style.alignment = TextAnchor.UpperLeft;
            GUI.Label(new Rect(10+2, 10+2, 500, 30), fpsText, style);
            style.normal.textColor = Color.white;
            GUI.Label(new Rect(10, 10, 500, 30), fpsText, style);
        }

        private static void Update()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText = $"FPS: {Mathf.Ceil(fps)}";
        }
    }
}
