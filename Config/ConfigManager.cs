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
        private readonly IYamlTypeConverter _customConverter;
        
        public ConfigManager(string configPath = "config.yml", IYamlTypeConverter customConverter = null)
        {
            _configPath = configPath;
            _customConverter = customConverter;
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
        
        private ISerializer CreateSerializer()
        {
            var builder = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance);

            if (_customConverter != null)
            {
                builder = builder.WithTypeConverter(_customConverter);
            }

            return builder.Build();
        }

        private IDeserializer CreateDeserializer()
        {
            var builder = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance);

            if (_customConverter != null)
            {
                builder = builder.WithTypeConverter(_customConverter);
            }

            return builder.Build();
        }

        private void SaveConfig()
        {
            var directory = Path.GetDirectoryName(_configPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var serializer = CreateSerializer();
            var yaml = serializer.Serialize(_config);
            File.WriteAllText(_configPath, yaml);
        }

        
        private void LoadConfig()
        {
            var deserializer = CreateDeserializer();
            var yaml = File.ReadAllText(_configPath);
            _config = deserializer.Deserialize<T>(yaml);
        }
    }
}