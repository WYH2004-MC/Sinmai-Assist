using UnityEngine;

namespace SinmaiAssist.GUI;

public class Style
{
    public readonly GUIStyle Title = new GUIStyle()
    {
        fontSize = 12,
        alignment = TextAnchor.MiddleCenter,
        normal = new GUIStyleState() { textColor = Color.white }
    };
    
    public readonly GUIStyle DisablePanel = new GUIStyle()
    {
        fontSize = 40,
        alignment = TextAnchor.MiddleCenter,
        normal = new GUIStyleState(){textColor = Color.white}
    };

    public readonly GUIStyle ErrorMessage = new GUIStyle()
    {
        normal = new GUIStyleState() { textColor = Color.red }
    };

    public GUIStyle Text
    {
        get
        {
            return new GUIStyle(UnityEngine.GUI.skin.label)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleLeft
            };
        }
    }
}