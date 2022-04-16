using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace StacklandsChinese
{
    public static class I18NMisc
    {
        [HarmonyPrefix, HarmonyPatch(typeof(CardData), "FullName", MethodType.Getter)]
        public static bool CardDataFullNamePatch(CardData __instance, ref string __result)
        {
            string text = __instance.Name;
            if (__instance.MyCardType == CardType.Ideas)
            {
                text = "点子: " + text;
            }
            if (__instance.IsFoil)
            {
                text = text + " (闪亮)";
            }
            __result = text;
            return false;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(Combatable), "GetCombatDescription")]
        public static IEnumerable<CodeInstruction> CombatableGetCombatDescriptionPatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Attack Speed: {0}\nHit Chance: {1}% \nDamage: {2}-{3}", "攻击速度：{0}\n命中率：{1}%\n伤害：{2}-{3}");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(Blueprint), "GetText")]
        public static IEnumerable<CodeInstruction> BlueprintGetTextPatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Stack ", "堆叠 ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(Subprint), "DefaultText")]
        public static IEnumerable<CodeInstruction> SubprintDefaultTextPatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, " and ", " 和 ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(AchievementElement), "UpdateVisuals")]
        public static IEnumerable<CodeInstruction> AchievementElementUpdateVisualsPatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Quest", "任务");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Complete more quests to see this one!", "完成更多的任务来查看此任务!");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Not possible in Peaceful Mode", "在和平模式下无法完成");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(Boosterpack), "GetSummaryFromAllCards")]
        public static IEnumerable<CodeInstruction> BoosterpackGetSummaryFromAllCardsPatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "An Idea", "一个点子");
            instructions = StacklandsChinese.ReplaceIL(instructions, "May contain: \n", "可能包含：\n");
            instructions = StacklandsChinese.ReplaceIL(instructions, "  • 1 undiscovered Card\n", " • 1张未被发现的卡\n");
            instructions = StacklandsChinese.ReplaceIL(instructions, "  • {0} undiscovered Cards\n", " • {0}张未被发现的卡\n");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(BuyBoosterBox), "GetTooltipText")]
        public static IEnumerable<CodeInstruction> BuyBoosterBoxGetTooltipTextPatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Drag {0}{1} here to buy the {2} Pack.\n\n{3}", "把{0}{1}拖到这里来买{2}包。\n\n{3}");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Complete {0} more quests to unlock this Pack", "再完成{0}个任务来解锁这个卡包");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(GameOverScreen), "Update")]
        public static IEnumerable<CodeInstruction> GameOverScreenUpdatePatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "You reached Moon {0}\n", "你度过了{0}月\n");
            instructions = StacklandsChinese.ReplaceIL(instructions, "1 Quest Completed\n", "完成了1个任务\n");
            instructions = StacklandsChinese.ReplaceIL(instructions, "{0} Quests Completed\n", "完成了{0}个任务\n");
            instructions = StacklandsChinese.ReplaceIL(instructions, "1 New Card Found\n", "收集了1个新卡片\n");
            instructions = StacklandsChinese.ReplaceIL(instructions, "{0} New Cards Found\n", "收集了{0}个新卡片\n");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(IdeaElement), "Update")]
        public static IEnumerable<CodeInstruction> IdeaElementUpdatePatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Buy Packs to find this Idea!", "购买卡包来发现更多点子!");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestsScreen), "Update")]
        public static IEnumerable<CodeInstruction> QuestsScreenUpdatePatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Complete {0} more quests to unlock a new Pack!", "再完成{0}个任务来解锁一个新的卡包！");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(BlueprintGrowth), "PopulateSubprints")]
        public static IEnumerable<CodeInstruction> BlueprintGrowthPopulateSubprintsPatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Growing a Berry Bush", "种植浆果丛");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Growing an Apple Tree", "种植苹果树");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Growing Carrots", "种植胡萝卜");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Growing Onions", "种植洋葱");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Growing Potatoes", "种植马铃薯");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Growing Mushrooms", "种植蘑菇");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(Brickyard), "Update")]
        public static IEnumerable<CodeInstruction> BrickyardUpdatePatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Making Brick", "制作砖块");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(Corpse), "Update")]
        public static IEnumerable<CodeInstruction> CorpseUpdatePatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Creating Graveyard", "创建墓地");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(House), "Update")]
        public static IEnumerable<CodeInstruction> HouseUpdatePatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Growing up", "长大");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(Sawmill), "Update")]
        public static IEnumerable<CodeInstruction> SawmillUpdatePatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Making Plank", "制作木板");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(StrangePortal), "Update")]
        public static IEnumerable<CodeInstruction> StrangePortalUpdatePatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "The Portal is Shaking", "入口在晃动");
            return instructions;
        }
    }
}
