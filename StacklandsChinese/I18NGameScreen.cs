using TMPro;
using HarmonyLib;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace StacklandsChinese
{
    public static class I18NGameScreen
    {
        [HarmonyPostfix, HarmonyPatch(typeof(GameScreen), "Start")]
        public static void StartPatch(GameScreen __instance)
        {
            __instance.QuestsButton.TextMeshPro.text = "任务";
            __instance.IdeasButton.TextMeshPro.text = "点子";
            __instance.ShowInfoBoxFood.InfoBoxTitle = "食物";
            __instance.ShowInfoBoxCard.InfoBoxTitle = "卡片上限";
            __instance.ShowInfoBoxTime.InfoBoxTitle = "时间";
            var moneyInfoBox = __instance.transform.Find("ResourcesBackground/MoneyBackground").GetComponent<ShowInfoBox>();
            moneyInfoBox.InfoBoxTitle = "金币";
            moneyInfoBox.InfoBoxText = "你拥有的金币总数";
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(GameScreen), "ShowAchievementComplete")]
        public static IEnumerable<CodeInstruction> ShowAchievementCompletePatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Quest Complete!", "任务完成!");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(GameScreen), "Update")]
        public static IEnumerable<CodeInstruction> UpdatePatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Maximize", "放大");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Press this button or press [Q] to maximize the Quests and Ideas Tab", "按此按钮或按[Q]键放大任务和点子界面");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Minimize", "缩小");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Press this button or press [Q] to minimize the Quests and Ideas Tab", "按此按钮或按[Q]键放大任务和点子界面");
            instructions = StacklandsChinese.ReplaceIL(instructions, "The current Time. Use [Space] to pause or use [Tab] to toggle between Game Speeds", "当前时间。使用[Space]暂停或使用[Tab]在游戏速度之间切换");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Every month you need {0} {1} to feed your Humans. You currently have {2} {3}", "每个月你都需要{0}{1}来养活你的村民。你现在有{2}{3}");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Your current maximum amount of Cards is {0}. You have {1} {2} (does not include Coins)", "您当前的最大卡片数为{0}。你有{1}{2}（不包括金币）");
            instructions = StacklandsChinese.ReplaceIL(instructions, " Get ", " 获得 ");
            instructions = StacklandsChinese.ReplaceIL(instructions, " so you won't lose your Humans!", " 所以你没有失去你的村民!");
            instructions = StacklandsChinese.ReplaceIL(instructions, "You have too many ", "你拥有太多的 ");
            instructions = StacklandsChinese.ReplaceIL(instructions, ". Get rid of Cards or you'll have to remove Cards at the end of the Moon!", ". 扔掉卡片，否则你将不得不在月末移除卡片!");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Moon {0}", "月 {0}");
            instructions = StacklandsChinese.ReplaceIL(instructions, " - Peaceful Mode", " - 和平模式");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Complete {0} more quests to unlock a new Pack!", "再完成{0}个任务来解锁一个新的背包!");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Click this Pack to get Cards!", "点击此包获取卡片!");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Can't be sold", "无法出售");
            instructions = StacklandsChinese.ReplaceIL(instructions, "Stack of cards", "一叠卡片");
            instructions = StacklandsChinese.ReplaceIL(instructions, "\nDrop to sell for {0}!", "\n以{0}的价格出售!");
            return instructions;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(GameScreen), "UpdateIdeasLog")]
        public static bool UpdateIdeasLogPatch(GameScreen __instance)
        {
            foreach (object obj in __instance.IdeaElementsParent)
            {
                Transform transform = (Transform)obj;
                if (!transform.name.StartsWith("DontDestroy"))
                {
                    GameObject.Destroy(transform.gameObject);
                }
            }
            List<Blueprint> list = WorldManager.instance.BlueprintPrefabs;
            list = (from x in list
                    orderby __instance.groups.IndexOf(x.BlueprintGroup)
                    select x).ToList<Blueprint>();
            int count = list.Count;
            int num = 0;
            Blueprint blueprint = null;
            for (int i = 0; i < list.Count; i++)
            {
                if (blueprint == null || blueprint.BlueprintGroup != list[i].BlueprintGroup)
                {
                    GameObject gameObject = GameObject.Instantiate<GameObject>(GameCanvas.instance.AchievementElementLabel);
                    gameObject.transform.SetParent(__instance.IdeaElementsParent);
                    gameObject.transform.localScale = Vector3.one;
                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.transform.localRotation = Quaternion.identity;
                    string label = StacklandsChinese.GetI18N($"BlueprintGroup_{list[i].BlueprintGroup}", list[i].BlueprintGroup.ToString());
                    gameObject.GetComponentInChildren<TextMeshProUGUI>().text = label;
                }
                IdeaElement ideaElement = GameObject.Instantiate<IdeaElement>(__instance.IdeaElementPrefab);
                ideaElement.transform.SetParent(__instance.IdeaElementsParent);
                ideaElement.transform.localScale = Vector3.one;
                ideaElement.transform.localRotation = Quaternion.identity;
                ideaElement.transform.localPosition = Vector3.zero;
                ideaElement.SetIdea(list[i]);
                if (ideaElement.WasFound)
                {
                    num++;
                }
                blueprint = list[i];
            }
            __instance.IdeasFoundText.text = string.Format("{0}/{1} 点子已收集", num, count);
            return false;
        }
    }
}
