using TMPro;
using HarmonyLib;
using UnityEngine;

namespace StacklandsChinese
{
    public static class I18NFont
    {
        [HarmonyPostfix, HarmonyPatch(typeof(FontManager), "Awake")]
        public static void FontPatch(FontManager __instance)
        {

            __instance.RegularFontAsset = StacklandsChinese.TMPTranslateFont;
            __instance.TitleFontAsset = StacklandsChinese.TMPTranslateFont;
        }

        /// <summary>
        /// 修改TMP字体
        /// </summary>
        [HarmonyPostfix, HarmonyPatch(typeof(TextMeshProUGUI), "OnEnable")]
        public static void TMPFontPatch(TextMeshProUGUI __instance)
        {
            if (__instance.font.name != StacklandsChinese.TMPTranslateFont.name)
            {
                __instance.font = StacklandsChinese.TMPTranslateFont;
            }
        }

        /// <summary>
        /// 修改TMP字体
        /// </summary>
        [HarmonyPostfix, HarmonyPatch(typeof(TextMeshPro), "OnEnable")]
        public static void TMPFontPatch2(TextMeshPro __instance)
        {
            if (__instance.font.name != StacklandsChinese.TMPTranslateFont.name)
            {
                __instance.font = StacklandsChinese.TMPTranslateFont;
            }
        }

        /// <summary>
        /// 如果有不显示的文本，则设置显示方式为溢出
        /// </summary>
        [HarmonyPostfix, HarmonyPatch(typeof(TextMeshProUGUI), "InternalUpdate")]
        public static void TMPFontPatch2(TextMeshProUGUI __instance)
        {
            if (__instance.font == StacklandsChinese.TMPTranslateFont)
            {
                if (__instance.overflowMode != TextOverflowModes.Overflow)
                {
                    if (__instance.preferredWidth > 1 && __instance.bounds.extents == Vector3.zero)
                    {
                        __instance.overflowMode = TextOverflowModes.Overflow;
                    }
                }
            }
        }
    }
}
