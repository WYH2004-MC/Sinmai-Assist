using System;

namespace SinmaiAssist.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class EnableGameVersionAttribute(uint minGameVersion, uint maxGameVersion = 99999): Attribute
{
    public uint MinGameVersion { get; } = minGameVersion;
    public uint MaxGameVersion { get; } = maxGameVersion;
        
    public bool ShouldEnable() 
    { 
        if (SinmaiAssist.GameVersion >= MinGameVersion && SinmaiAssist.GameVersion <= MaxGameVersion)
        {
            return true; 
        } 
        return false;
    }
}