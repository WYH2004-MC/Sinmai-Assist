using System;
using System.IO;
using System.Linq;
using System.Reflection;
using DB;
using HarmonyLib;
using MAI2.Util;
using Manager;
using Manager.UserDatas;
using MelonLoader;
using SinmaiAssist.Utils;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SinmaiAssist.Common;

public class ChangeDefaultOption
{
    private static string OptionFilePath = $"{BuildInfo.Name}/DefaultOption.yml";

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UserDataManager), "SetDefault")]
    public static void DefaultOption(UserDataManager __instance, ref long index)
    {
        var userData = __instance.GetUserData(index);
        if (!userData.IsGuest()) return;
        try
        {
            UserOption option = Singleton<UserDataManager>.Instance.GetUserData(index).Option;
            option.OptionKind = OptionKindID.Custom;
            if (!File.Exists(OptionFilePath))
            {
                MelonLogger.Warning($"Path: \"{OptionFilePath}\" Not Found, Try to create it.");
                SaveOptionFile(index);
            }
            else
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                using (var reader = new StreamReader(OptionFilePath))
                {
                    var newOption = deserializer.Deserialize<UserOption>(reader);
                    UpdateUserOption(option, newOption);
                }
            }
        }
        catch (Exception e)
        {
            MelonLogger.Error(e);
        }
    }

    private static void UpdateUserOption(UserOption originalOption, UserOption newOption)
    {
        if (newOption == null) return;
        PropertyInfo[] properties = typeof(UserOption).GetProperties();
        foreach (var property in properties)
        {
            if (property.CanWrite)
            {
                var newValue = property.GetValue(newOption);
                property.SetValue(originalOption, newValue);
            }
        }
    }

    private static string RemoveUnwantedOption(string yaml)
    {
        string[] propertiesToRemove = { "getNoteSpeed", "getSlideSpeed", "getTouchSpeed" };
        var lines = yaml.Split('\n');
        var filteredLines = lines.Where(line => !propertiesToRemove.Any(prop => line.TrimStart().StartsWith(prop)));
        return string.Join("\n", filteredLines);
    }

    public static void SaveOptionFile(long index)
    {
        UserOption option = Singleton<UserDataManager>.Instance.GetUserData(index).Option;
        option.OptionKind = OptionKindID.Custom;
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        string optionYaml = serializer.Serialize(option);
        optionYaml = RemoveUnwantedOption(optionYaml);
        if (File.Exists(OptionFilePath))
        {
            File.Delete(OptionFilePath);
        }
        File.WriteAllText(OptionFilePath, optionYaml);
        GameMessageManager.SendMessage((int)index,$"User Option File Saved");
    }
}