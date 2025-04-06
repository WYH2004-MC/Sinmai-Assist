using System;

namespace SinmaiAssist.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class EnableGameVersionAttribute(uint minGameVersion, uint maxGameVersion): Attribute
{
    public uint MinGameVersion { get; } = minGameVersion;
    public uint MaxGameVersion { get; } = maxGameVersion;
        
    public bool ShouldEnable() 
    { 
        if (SinmaiAssist.gameVersion >= MinGameVersion && SinmaiAssist.gameVersion <= MaxGameVersion)
        {
            return true; 
        } 
        return false;
    }
}