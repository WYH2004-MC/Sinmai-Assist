using System;
using System.IO;
using System.Linq;
using System.Reflection;
using MelonLoader;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SinmaiAssist.Config
{
    public class ConfigManager<T> where T : new()
    {
        private readonly string _configPath;
        private T _config;
        
        public ConfigManager(string configPath = "config.yml")
        {
            _configPath = configPath;
            InitConfig();
        }

        private void InitConfig()
        {
            try
            {
                if (!File.Exists(_configPath))
                {
                    _config = new T();
                    SaveConfig();
                    MelonLogger.Msg($"Create Default Config '{_configPath}' ");
                    return;
                }

                LoadConfig();
            }
            catch (YamlException ex)
            {
                MelonLogger.Error($"Load Config '{_configPath}' Failed: \n{ex.Message}");
                MelonLogger.Warning($"Your Config is not valid, please delete it and restart the game.");
                throw;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Init Config '{_configPath}' Failed: \n{ex.Message}");
                throw;
            }
        }

        public T GetConfig()
        {
            if (_config == null)
            {
                throw new InvalidOperationException("Configuration is not initialized.");
            }
            return _config;
        }

        private void SaveConfig()
        {
            var directory = Path.GetDirectoryName(_configPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var yaml = serializer.Serialize(_config);
            File.WriteAllText(_configPath, yaml);
        }

        
        private void LoadConfig()
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var yaml = File.ReadAllText(_configPath);
            _config = deserializer.Deserialize<T>(yaml);
        }
    }
}