using TMPro;
using HarmonyLib;

namespace StacklandsChinese
{
    public static class I18NRunOptionsScreen
    {
        [HarmonyPostfix, HarmonyPatch(typeof(RunOptionsScreen), "Start")]
        public static void StartPatch(RunOptionsScreen __instance)
        {
            __instance.transform.Find("Background/OptionsTitle").GetComponent<TextMeshProUGUI>().text = "游戏设置";
            __instance.transform.Find("Background/Buttons/Label").GetComponent<TextMeshProUGUI>().text = "每月长度";
            __instance.transform.Find("Background/Buttons/PeacefulMode").GetComponent<TextMeshProUGUI>().text = "和平模式";
            __instance.transform.Find("Background/Buttons/MoonLengthButtons").GetComponent<ShowTooltip>().MyTooltipText = "每个月的时间长短，越长越简单";
            __instance.transform.Find("Background/Buttons/PeacefulModeButtons").GetComponent<ShowTooltip>().MyTooltipText = "和平模式下没有敌人";
            __instance.LongMoon.TextMeshPro.text = "较长";
            __instance.NormalMoon.TextMeshPro.text = "普通";
            __instance.ShortMoon.TextMeshPro.text = "较短";
            __instance.PeacefulModeOn.TextMeshPro.text = "开";
            __instance.PeacefulModeOff.TextMeshPro.text = "关";
            __instance.PlayButton.TextMeshPro.text = "开始游戏";
        }
    }
}
