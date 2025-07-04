using System;
using MelonLoader;
using UnityEngine;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace SinmaiAssist.Config;

public class KeyBindConfig
{
    public AutoPlayConfig AutoPlay { get; set; } = new AutoPlayConfig();
    public ChartControllerConfig ChartController { get; set; } = new ChartControllerConfig();
    public SinmaiAssistConfig SinmaiAssist { get; set; } = new SinmaiAssistConfig();

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
        private string _keyCode;

        public Key(string key)
        {
            _keyCode = key;
        }

        public static implicit operator Key(string key) => new Key(key);
        public static implicit operator string(Key key) => key._keyCode;
        public new string ToString() => _keyCode;
        
        public KeyCode KeyCode
        {
            get
            {
                if (_keyCode == "None")
                    return UnityEngine.KeyCode.None;
                if (Enum.TryParse<KeyCode>(_keyCode, out var code))
                    return code;
                
                return KeyCode.None;
            }
        }
    }

    public class Converter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(Key);
        }
        
        public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
        {
            var scalar = (Scalar) parser.Current;
            var value = scalar.Value;
            parser.MoveNext();
            return new Key(value);
        }

        public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
        {
            if (value is Key key)
            {
                emitter.Emit(new Scalar(key.ToString()));
            }
            else
            {
                emitter.Emit(new Scalar("None"));
            }
        }
    }
}