using System;
using MelonLoader;
using UnityEngine;

namespace SinmaiAssist.Config;

public class KeyBindConfig
{
    public AutoPlayConfig AutoPlay { get; set; } = new AutoPlayConfig();
    public ChartControllerConfig ChartController { get; set; } = new ChartControllerConfig();
    public SinmaiAssistConfig SinaiAssist { get; set; } = new SinmaiAssistConfig();

    public class AutoPlayConfig
    {
        public Key None { get; set; } = "N";
        public Key Critical { get; set; } = "F";
        public Key Perfect { get; set; } = "None";
        public Key Great { get; set; } = "O";
        public Key Good { get; set; } = "P";
        public Key Random { get; set; } = "K";
        public Key RandomAllPerfect { get; set; } = "G";
        public Key RandomFullComboPlus { get; set; } = "H";
        public Key RandomFullCombo { get; set; } = "J";
    }
    
    public class ChartControllerConfig
    {
        public Key Pause { get; set; } = "Enter";
        public Key Forward { get; set; } = "RightArrow";
        public Key Backward { get; set; } = "LeftArrow";
        public Key SetRecord { get; set; } = "DownArrow";
        public Key ReturnRecord { get; set; } = "UpArrow";
    }

    public class SinmaiAssistConfig
    {
        public Key ShowUserPanel { get; set; } = "Backspace";
    }
    
    public class Key
    {
        private string _key;
        
        public Key(string key)
        {
            _key = key;
        }

        public static implicit operator Key(string key)
        {
            return new Key(key);
        }
        
        public static implicit operator string (Key key)
        {
            return key._key;
        }
        
        public KeyCode KeyCode
        {
            get
            {
                if (_key == "None")
                    return KeyCode.None;
                if (Enum.TryParse<KeyCode>(_key, out var code))
                    return code;
                
                return KeyCode.None;
            }
        }
        
        public new string ToString()
        {
            return _key;
        }
    }
}