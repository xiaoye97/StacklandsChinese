using TMPro;
using HarmonyLib;
using System.Collections.Generic;

namespace StacklandsChinese
{
    public static class I18NMainMenu
    {
        [HarmonyPostfix, HarmonyPatch(typeof(MainMenu), "Start")]
        public static void StartPatch(MainMenu __instance)
        {
            __instance.transform.Find("Background/GameTitle").GetComponent<TextMeshProUGUI>().text = "堆叠大陆\n<size=18>汉化:bilibili 宵夜97</size>";
            __instance.NewGameButton.TextMeshPro.text = "开始新游戏";
            __instance.CardopediaButton.TextMeshPro.text = "卡片百科";
            __instance.OptionsButton.TextMeshPro.text = "设置";
            __instance.QuitButton.TextMeshPro.text = "退出游戏";
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(MainMenu), "Update")]
        public static IEnumerable<CodeInstruction> UpdatePatch(IEnumerable<CodeInstruction> instructions)
        {
            return StacklandsChinese.ReplaceIL(instructions, "Continue Run (Moon {0})", "继续游戏({0}月)");
        }
    }
}
