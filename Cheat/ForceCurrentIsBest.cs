using HarmonyLib;
using Manager.UserDatas;
using MelonLoader;
using Process;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SinmaiAssist.Cheat;

public class ForceCurrentIsBest
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(ResultProcess), "OnStart")]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        bool isFound = false;

        var targetMethod = typeof(List<UserScore>).GetMethod("Find", new[] { typeof(Predicate<UserScore>) });

        for (int i = 0; i < codes.Count - 1; i++)
        {
            if (codes[i].opcode == OpCodes.Callvirt && codes[i].operand == targetMethod)
            {
                codes[i] = new CodeInstruction(OpCodes.Ldnull);
                codes.RemoveRange(i - 10, 10);
                isFound = true;
                break;
            }
        }
        if (!isFound)
            MelonLogger.Warning("Failed to patch ForceCurrentIsBest, Method Not Found!");

        return codes;
    }
}