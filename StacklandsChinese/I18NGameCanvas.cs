using System;
using HarmonyLib;
using System.Collections.Generic;

namespace StacklandsChinese
{
    public static class I18NGameCanvas
    {
        [HarmonyPrefix, HarmonyPatch(typeof(GameCanvas), "GetAchievementGroupName")]
        public static bool GetAchievementGroupNamePatch(GameCanvas __instance, AchievementGroup group, ref string __result)
        {
            __result = StacklandsChinese.GetI18N($"AchievementGroup_{group}", group.ToString());
            return false;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(GameCanvas), "FormatTimeLeft")]
        public static IEnumerable<CodeInstruction> GameCanvasFormatTimeLeftPatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "s left", "s 剩余");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(GameCanvas), "ShowClearSaveModal")]
        public static IEnumerable<CodeInstruction> GameCanvasShowClearSaveModalPatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Clear Save", "清除存档");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Are you sure you want to clear your save?", "确定清除你的存档吗?");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Yes", "是");
            instructions = StacklandsChinese.ReplaceIL(instructions, "No", "否");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(GameCanvas), "ShowRestartModal")]
        public static IEnumerable<CodeInstruction> GameCanvasShowRestartModalPatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Restart Run", "重新开始");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Are you sure you want to restart this run?", "确定要重新开始本局游戏吗?");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Yes", "是");
            instructions = StacklandsChinese.ReplaceIL(instructions, "No", "否");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(GameCanvas), "ShowStartNewRunModal")]
        public static IEnumerable<CodeInstruction> GameCanvasShowStartNewRunModalPatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Start New Run", "开始新游戏");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Are you sure you want to start a new run? You'll lose the progress in your current run!", "确定要开始一局新的游戏吗? 你会丢失上一局游戏的进度!");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Yes", "是");
            instructions = StacklandsChinese.ReplaceIL(instructions, "No", "否");
            return instructions;
        }
    }
}
