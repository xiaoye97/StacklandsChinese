using TMPro;
using HarmonyLib;

namespace StacklandsChinese
{
    public static class I18NSelectResolutionScreen
    {
        [HarmonyPostfix, HarmonyPatch(typeof(SelectResolutionScreen), "Start")]
        public static void StartPatch(SelectResolutionScreen __instance)
        {
            __instance.transform.Find("Background/Title").GetComponent<TextMeshProUGUI>().text = "分辨率";
            __instance.BackButton.TextMeshPro.text = "返回";
        }
    }
}
