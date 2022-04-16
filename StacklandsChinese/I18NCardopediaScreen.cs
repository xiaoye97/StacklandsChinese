using TMPro;
using HarmonyLib;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace StacklandsChinese
{
    public static class I18NCardopediaScreen
    {
        [HarmonyPostfix, HarmonyPatch(typeof(CardopediaScreen), "Awake")]
        public static void AwakePatch(CardopediaScreen __instance)
        {
            __instance.transform.Find("Background/Title").GetComponent<TextMeshProUGUI>().text = "卡片百科";
            __instance.BackButton.TextMeshPro.text = "返回";
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(CardopediaScreen), "Update")]
        public static IEnumerable<CodeInstruction> UpdatePatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Card not yet found", "未发现的卡片");
            instructions = StacklandsChinese.ReplaceIL(instructions, "{0}/{1} Cards Found", "{0}/{1} 已发现卡片");
            return instructions;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(CardopediaScreen), "CreateEntries")]
        public static bool CreateEntriesPatch(CardopediaScreen __instance)
        {
            List<CardData> list = WorldManager.instance.CardDataPrefabs;
            list = (from x in list
                    orderby x.MyCardType
                    select x).ToList<CardData>();
            new List<Transform>();
            foreach (object obj in __instance.EntriesParent)
            {
                GameObject.Destroy(((Transform)obj).gameObject);
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0 || list[i - 1].MyCardType != list[i].MyCardType)
                {
                    GameObject gameObject = GameObject.Instantiate<GameObject>(__instance.LabelPrefab);
                    gameObject.transform.SetParent(__instance.EntriesParent);
                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.transform.localScale = Vector3.one;
                    gameObject.transform.localRotation = Quaternion.identity;
                    string cardType = StacklandsChinese.GetI18N($"CardType_{list[i].MyCardType}", list[i].MyCardType.ToString());
                    gameObject.GetComponentInChildren<TextMeshProUGUI>().text = cardType;
                }
                CardData cardData = list[i];
                CardopediaEntryElement cardopediaEntryElement = GameObject.Instantiate<CardopediaEntryElement>(__instance.CardopediaEntryPrefab);
                cardopediaEntryElement.transform.SetParent(__instance.EntriesParent);
                cardopediaEntryElement.transform.localPosition = Vector3.zero;
                cardopediaEntryElement.transform.localScale = Vector3.one;
                cardopediaEntryElement.transform.localRotation = Quaternion.identity;
                cardopediaEntryElement.SetCardData(cardData);
                __instance.entries.Add(cardopediaEntryElement);
            }
            return false;
        }
    }
}
