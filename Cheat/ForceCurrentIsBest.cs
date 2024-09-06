using HarmonyLib;
using MelonLoader;
using Process;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Cheat
{
    public class ForceCurrentIsBest
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(ResultProcess), "OnStart")]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            bool isFound = false;
            CodeInstruction ldloc = null;
            CodeInstruction brtrue = null;

            // 遍历所有的 IL 指令
            for (int i = 0; i < codes.Count - 1; i++)
            {
                if (codes[i].opcode == OpCodes.Ldloc_S && codes[i + 1].opcode == OpCodes.Brtrue)
                {
                    if (codes[i].operand.ToString().Contains("Manager.UserDatas.UserScore"))
                    {
                        MelonLogger.Msg($"Found! {codes[i].operand}");
                        
                        codes[i+1].opcode = OpCodes.Pop;
                        codes[i+1].operand = null;

                        isFound = true;
                        break;
                    }
                }
            }
            if (!isFound)
                MelonLogger.Warning("Failed to patch ForceCurrentIsBest, 'if(userScore == null)' Not Found!");

            return codes.AsEnumerable();
        }
    }
}