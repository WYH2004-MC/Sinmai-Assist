using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MelonLoader;
using Process;

namespace SinmaiAssist.Fix;

public class DisableEnvironmentCheck
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(WarningProcess), "OnStart")]
    public static IEnumerable<CodeInstruction> OnStart(IEnumerable<CodeInstruction> instructions)
    {
        var codes = instructions.ToList();
        var onceDispIndex = codes.FindIndex(
            inst =>
                inst.opcode == OpCodes.Ldsfld &&
                inst.operand is FieldInfo field &&
                field.Name == "OnceDisp");
        if (onceDispIndex == -1)
        {
            MelonLogger.Warning("Failed to patch DisableEnvironmentCheck, Method Not Found!");
            return codes;
        }
        
        var skippedInstructions = codes.Take(onceDispIndex).ToList();
        MelonLogger.Msg("Disable Environment Check Skipped Code Output:");
        foreach (var inst in skippedInstructions)
        {
            MelonLogger.Msg($"[Disable Environment Check] Opcode: {inst.opcode}, Operand: {inst.operand}");
        }
        return codes.Skip(onceDispIndex);
    }
}