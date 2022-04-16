using TMPro;
using HarmonyLib;

namespace StacklandsChinese
{
    public static class I18NPauseScreen
    {
        [HarmonyPostfix, HarmonyPatch(typeof(PauseScreen), "Awake")]
        public static void AwakePatch(PauseScreen __instance)
        {
            __instance.transform.Find("Background/PauseTitle").GetComponent<TextMeshProUGUI>().text = "暂停";
            __instance.ContinueButton.GetComponentInChildren<TextMeshProUGUI>(true).text = "继续";
            __instance.CardopediaButton.GetComponentInChildren<TextMeshProUGUI>(true).text = "卡片百科";
            __instance.OptionsButton.GetComponentInChildren<TextMeshProUGUI>(true).text = "设置";
            __instance.MainMenuButton.GetComponentInChildren<TextMeshProUGUI>(true).text = "回到标题";
            __instance.QuestsButton.GetComponentInChildren<TextMeshProUGUI>(true).text = "任务";
            __instance.RestartRunButton.GetComponentInChildren<TextMeshProUGUI>(true).text = "重新开始";
        }
    }
}
