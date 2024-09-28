using HarmonyLib;
using MelonLoader;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SinmaiAssist.Common;

public class ChangeFadeStyle
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(FadeProcess), "OnStart")]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        bool isFound = false;
        
        for (int i = 0; i < codes.Count - 1; i++)
        {
            if (codes[i].opcode == OpCodes.Ldstr && codes[i].operand.ToString().StartsWith("Process/ChangeScreen/Prefabs/ChangeScreen_"))
            {
                var typeString = "02";
                if (codes[i].operand.ToString().EndsWith("02")) typeString = "01";
                codes[i].operand = $"Process/ChangeScreen/Prefabs/ChangeScreen_{typeString}";
                isFound = true;
                break;
            }
        }
        if (!isFound)
            MelonLogger.Warning("Failed to patch ChangeFadeStyle, Method Not Found!");

        return codes;
    }
}