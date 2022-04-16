using TMPro;
using HarmonyLib;

namespace StacklandsChinese
{
    public static class I18NOptionsScreen
    {
        [HarmonyPostfix, HarmonyPatch(typeof(OptionsScreen), "Awake")]
        public static void AwakePatch(OptionsScreen __instance)
        {
            __instance.transform.Find("Background/OptionsTitle").GetComponent<TextMeshProUGUI>().text = "设置";
            __instance.BackButton.GetComponentInChildren<TextMeshProUGUI>(true).text = "返回";
            __instance.ClearSaveButton.GetComponentInChildren<TextMeshProUGUI>(true).text = "清除存档";
            __instance.CreditsButton.GetComponentInChildren<TextMeshProUGUI>(true).text = "致谢";
        }

        [HarmonyPrefix, HarmonyPatch(typeof(OptionsScreen), "SetTexts")]
        public static bool SetTextsPatch(OptionsScreen __instance)
        {
            __instance.MusicButton.TextMeshPro.text = "音乐: " + (OptionsScreen.MusicOn ? "开" : "关");
            __instance.SfxButton.TextMeshPro.text = "音效: " + (OptionsScreen.SfxOn ? "开" : "关");
            __instance.ResolutionButton.TextMeshPro.text = string.Format("分辨率: {0}x{1}", OptionsScreen.CurrentWidth, OptionsScreen.CurrentHeight);
            __instance.FullscreenButton.TextMeshPro.text = (OptionsScreen.CurrentFullScreen ? "全屏" : "窗口");
            return false;
        }
    }
}
