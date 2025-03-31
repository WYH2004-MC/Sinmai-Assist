using System.Windows.Forms;
using Monitor;
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
    
    public readonly GUIStyle Button = new GUIStyle()
    {
        fontSize = 12,
        alignment = TextAnchor.MiddleCenter,
        border = new RectOffset(2, 2, 2, 2), //边框
        margin = new RectOffset(5,5,5,5), //边距
        padding = new RectOffset(4,4,4,4), //内边距
        normal = new GUIStyleState() 
        { 
            textColor = Color.white,
            background = CreateBorderTexture(new Color(0.2f, 0.2f, 0.2f,0.5f))
        },
        hover = new GUIStyleState() 
        { 
            textColor = Color.white,
            background = CreateBorderTexture(new Color(0.4f, 0.4f, 0.4f,0.5f))
        },
        active = new GUIStyleState()
        {
            textColor = Color.white,
            background = CreateBorderTexture(new Color(0.25f, 0.25f, 0.25f,0.5f))
        },
        onNormal = new GUIStyleState() { 
            textColor = Color.yellow,
            background = CreateBorderTexture(new Color(0.25f, 0.25f, 0.25f,0.5f))
        },
        onHover = new GUIStyleState() { 
            textColor = Color.yellow,
            background = CreateBorderTexture(new Color(0.4f, 0.4f, 0.4f,0.5f))
        }
        
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
    
    //创建背景纹理
    
    private static Texture2D CreateBorderTexture(Color color)
    {
        Texture2D texture = new Texture2D(3, 3)
        {
            wrapMode = TextureWrapMode.Repeat
        };
        for (int x = 0; x < 3; x++){
            for (int y = 0; y < 3; y++){
                texture.SetPixel(x, y, color);
            }
        }
        for (int i = 0; i < 3; i++){
            texture.SetPixel(i, 0, Color.white);
            texture.SetPixel(i, 2, Color.white);
            texture.SetPixel(0, i, Color.white);
            texture.SetPixel(2, i, Color.white);
        }
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }
}