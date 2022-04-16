using TMPro;
using HarmonyLib;

namespace StacklandsChinese
{
    public static class I18NSellBox
    {
        [HarmonyPrefix, HarmonyPatch(typeof(SellBox), "GetTooltipText")]
        public static bool GetTooltipTextPatch(SellBox __instance, ref string __result)
        {
            __result = "把卡片拖到这里来卖。你也可以使用[Backspace]来出售卡片。";
            return false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(SellBox), "Update")]
        public static void UpdatePatch(SellBox __instance)
        {
            if (__instance.name != "出售卡片")
            {
                __instance.name = "出售卡片";
                __instance.GetComponentInChildren<TextMeshPro>(true).text = "出售";
            }
        }
    }
}
