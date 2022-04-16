using TMPro;
using HarmonyLib;
using UnityEngine;

namespace StacklandsChinese
{
    public static class I18NCreditsScreen
    {
        [HarmonyPostfix, HarmonyPatch(typeof(CreditsScreen), "Start")]
        public static void StartPatch(CreditsScreen __instance)
        {
            __instance.transform.Find("Background/Credits").GetComponent<TextMeshProUGUI>().text = "致谢";
            __instance.BackButton.TextMeshPro.text = "返回";
            var xiaoyeBtnObj = __instance.transform.Find("Background/Buttons/XiaoyeBtn");
            if (xiaoyeBtnObj == null)
            {
                var go = GameObject.Instantiate(__instance.BackButton, __instance.transform.Find("Background/Buttons"));
                go.transform.SetAsFirstSibling();
                var btn = go.GetComponent<CustomButton>();
                btn.TextMeshPro.text = "宵夜97 - 汉化";
                btn.Clicked += () =>
                {
                    System.Diagnostics.Process.Start("https://space.bilibili.com/1306433");
                };
                btn.TooltipText = "https://space.bilibili.com/1306433";
            }
        }
    }
}
