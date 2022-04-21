using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StacklandsChinese
{
    public static class I18NWorldManager
    {
        [HarmonyTranspiler, HarmonyPatch(typeof(WorldManager), "QueueAchievementCompleteAnimation")]
        public static IEnumerable<CodeInstruction> QueueAchievementCompleteAnimationPatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "Quest Completed", "任务完成");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(WorldManager), "Update")]
        public static IEnumerable<CodeInstruction> UpdatePatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "All Humans Died", "所有村民都死了");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(WorldManager), "GetStackSummary")]
        public static IEnumerable<CodeInstruction> GetStackSummaryPatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, " and ", " 和 ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(WorldManager), "SetStarvingHumanStatus")]
        public static IEnumerable<CodeInstruction> SetStarvingHumanStatusPatch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = StacklandsChinese.ReplaceIL(instructions, "There is not enough Food.. {0} Human will starve of Hunger", "没有足够的食物。。{0}村民将饿死");
            instructions = StacklandsChinese.ReplaceIL(instructions, "There is not enough Food.. {0} Humans will starve of Hunger", "没有足够的食物。。{0}村民将饿死");
            return instructions;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(WorldManager), "Start")]
        public static void StartParch(WorldManager __instance)
        {
            foreach (var card in __instance.CardDataPrefabs)
            {
                card.Name = StacklandsChinese.GetI18N($"CardName_{card.Id}", card.Name);
                card.Description = StacklandsChinese.GetI18N($"CardDescription_{card.Id}", card.Description);
                if (card is CombatableHarvestable)
                {
                    CombatableHarvestable childClass = (CombatableHarvestable)card;
                    childClass.StatusText = StacklandsChinese.GetI18N($"CardStatusText_{card.Id}", childClass.StatusText);
                }
                if (card is Harvestable)
                {
                    Harvestable childClass = (Harvestable)card;
                    childClass.StatusText = StacklandsChinese.GetI18N($"CardStatusText_{card.Id}", childClass.StatusText);
                }
            }
            foreach (var prefab in __instance.BlueprintPrefabs)
            {
                prefab.StackPostText = StacklandsChinese.GetI18N($"BlueprintStackPostText_{prefab.Id}", prefab.StackPostText);
				if (prefab.Subprints != null && prefab.Subprints.Count > 0)
                {
					for (int i = 0; i < prefab.Subprints.Count; i++)
                    {
						var sub = prefab.Subprints[i];
						sub.StatusName = StacklandsChinese.GetI18N($"Blueprint_{prefab.Id}_Subprint_{i}_StatusName", sub.StatusName);
					}
                }
            }
            foreach (var prefab in __instance.BoosterPackPrefabs)
            {
                prefab.Name = StacklandsChinese.GetI18N($"BoosterPackName_{prefab.BoosterId}", prefab.Name);
                prefab.Description = StacklandsChinese.GetI18N($"BoosterPackDescription_{prefab.BoosterId}", prefab.Description);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(WorldManager), "EndOfMonthRoutine")]
        public static void EndOfMonthRoutinePatch(ref IEnumerator __result)
        {
			Debug.Log("测试输出:EndOfMonthRoutine");
			__result = EndOfMonthRoutine();
		}

		public static IEnumerator EndOfMonthRoutine()
        {
			var __instance = WorldManager.instance;
			AudioManager.me.PlaySound2D(AudioManager.me.EndOfMoon, 0.8f, 0.2f);
			__instance.ContinueButtonText = "喂食村民";
			__instance.EndOfMonthStatus = "吃饭时间";
			__instance.EndOfMonthText = string.Format("{0}月底", __instance.CurrentMonth - 1);
			__instance.ShowContinueButton = true;
			while (!__instance.ContinueClicked)
			{
				yield return null;
			}
			__instance.ShowContinueButton = false;
			foreach (TravellingCart travellingCart in __instance.GetCards<TravellingCart>())
			{
				travellingCart.MyGameCard.DestroyCard(true, true);
			}
			__instance.InEatingAnimation = true;
			__instance.EndOfMonthStatus = "正在吃饭..";
			int requiredFoodCount = __instance.GetRequiredFoodCount();
			List<CardData> villagersToFeed = new List<CardData>();
			foreach (GameCard gameCard in __instance.AllCards)
			{
				if (gameCard.CardData is Villager || gameCard.CardData is Kid)
				{
					villagersToFeed.Add(gameCard.CardData);
				}
			}
			villagersToFeed = villagersToFeed.OrderBy(delegate (CardData x)
			{
				if (!(x is Kid))
				{
					return 1;
				}
				return 0;
			}).ToList<CardData>();
			List<CardData> fedVillagers = new List<CardData>();
			yield return new WaitForSeconds(1f);
			__instance.FoodAnimSpeedup = 0f;
			int num;
			for (int i = 0; i < villagersToFeed.Count; i = num + 1)
			{
				CardData vill = villagersToFeed[i];
				__instance.FoodAnimSpeedup += 1f;
				int foodForVillager = __instance.GetCardFoodCount(vill.MyGameCard);
				for (int j = 0; j < foodForVillager; j = num + 1)
				{
					Food food = __instance.GetFoodToUseUp();
					if (food == null)
					{
						break;
					}
					GameCard foodCard = food.MyGameCard;
					foodCard.PushEnabled = false;
					foodCard.SetY = false;
					foodCard.Velocity = null;
					GameCamera.instance.TargetPositionOverride = new Vector3?(vill.transform.position);
					GameCard originalParent = foodCard.Parent;
					GameCard originalChild = foodCard.Child;
					Vector3 originalPosition = foodCard.TargetPosition;
					List<GameCard> originalStack = foodCard.GetAllCardsInStack();
					foodCard.RemoveFromStack();
					foodCard.TargetPosition = vill.transform.position + new Vector3(0f, 0.1f, 0f);
					Vector3 diff;
					do
					{
						diff = foodCard.TargetPosition - foodCard.transform.position;
						yield return null;
					}
					while (diff.magnitude > 0.001f);
					AudioManager.me.PlaySound2D(AudioManager.me.Eat, Random.Range(0.8f, 1.2f), 0.3f);
					food.FoodValue--;
					num = requiredFoodCount;
					requiredFoodCount = num - 1;
					foodCard.SetHitEffect(null);
					foodCard.transform.localScale *= 0.9f;
					yield return new WaitForSeconds(Mathf.Max(0.01f, 0.12f - __instance.FoodAnimSpeedup * 0.04f));
					Combatable combatable = vill as Combatable;
					if (combatable != null)
					{
						combatable.HealthPoints = Mathf.Min(combatable.HealthPoints + 1, combatable.MaxHealth);
					}
					if (food.FoodValue <= 0)
					{
						originalStack.Remove(foodCard);
						__instance.Restack(originalStack);
						foodCard.DestroyCard(true, true);
					}
					else
					{
						foodCard.PushEnabled = true;
						foodCard.SetY = true;
						if (originalParent != null)
						{
							foodCard.Parent = originalParent;
							originalParent.Child = foodCard;
						}
						if (originalChild != null)
						{
							foodCard.Child = originalChild;
							originalChild.Parent = foodCard;
						}
						foodCard.TargetPosition = originalPosition;
					}
					if (j == foodForVillager - 1)
					{
						fedVillagers.Add(vill);
					}
					food = null;
					foodCard = null;
					originalParent = null;
					originalChild = null;
					originalPosition = default(Vector3);
					originalStack = null;
					diff = default(Vector3);
					num = j;
				}
				vill = null;
				num = i;
			}
			yield return new WaitForSeconds(1f);
			__instance.InEatingAnimation = false;
			int num2 = requiredFoodCount;
			List<CardData> unfedVillagers = new List<CardData>();
			foreach (CardData cardData in villagersToFeed)
			{
				if (!fedVillagers.Contains(cardData) && !(cardData is Kid))
				{
					unfedVillagers.Add(cardData);
				}
			}
			int humansToDie = unfedVillagers.Count;
			if (num2 <= 0)
			{
				__instance.EndOfMonthStatus = "所有村民都吃饱了!";
			}
			else
			{
				__instance.SetStarvingHumanStatus(humansToDie);
				__instance.ShowContinueButton = true;
				__instance.ContinueButtonText = "啊哦";
				while (!__instance.ContinueClicked)
				{
					yield return null;
				}
				__instance.ShowContinueButton = false;
				for (int i = 0; i < unfedVillagers.Count; i = num + 1)
				{
					CardData cardData2 = unfedVillagers[i];
					if (!(cardData2 is Kid))
					{
						yield return __instance.KillVillagerCoroutine(cardData2 as Villager, null);
						__instance.SetStarvingHumanStatus(humansToDie - i);
					}
					num = i;
				}
				if (__instance.CheckVillagersDead())
				{
					__instance.EndOfMonthStatus = "所有村民都饿死了";
					__instance.GameOverReason = __instance.EndOfMonthStatus;
					__instance.ContinueButtonText = "游戏结束";
					__instance.ShowContinueButton = true;
					while (!__instance.ContinueClicked)
					{
						yield return null;
					}
					GameCanvas.instance.SetScreen(GameCanvas.instance.GameOverScreen);
					__instance.currentAnimationRoutine = null;
					yield break;
				}
			}
			yield return new WaitForSeconds(1.5f);
			int cardCount = __instance.GetCardCount();
			int maxCardCount = __instance.GetMaxCardCount();
			int num3 = cardCount - maxCardCount;
			if (num3 > 0)
			{
				__instance.EndOfMonthStatus = string.Format("你拥有的{0}张卡片太多了!", num3);
				__instance.ContinueButtonText = string.Format("出售{0}张卡片", num3);
				__instance.ShowContinueButton = true;
				while (!__instance.ContinueClicked)
				{
					yield return null;
				}
				__instance.ContinueClicked = false;
				__instance.ShowContinueButton = false;
				__instance.RemovingCards = true;
				while (__instance.GetCardCount() > maxCardCount)
				{
					GameCamera.instance.TargetPositionOverride = null;
					int num4 = __instance.GetCardCount() - maxCardCount;
					__instance.EndOfMonthStatus = string.Format("你拥有的{0}张卡片太多了", num4);
					__instance.EndOfMonthText = "出售你的卡片才能继续";
					yield return null;
				}
				__instance.RemovingCards = false;
			}
			__instance.EndOfMonthText = string.Format("开始第{0}月", __instance.CurrentMonth);
			bool flag = __instance.CurrentMonth > 6 && __instance.CurrentMonth % 6 == 0;
			if (__instance.CurrentRunOptions.IsPeacefulMode)
			{
				flag = false;
			}
			if (flag)
			{
				Vector3 randomSpawnPosition = __instance.GetRandomSpawnPosition();
				CardData cardData3 = __instance.CreateCard(randomSpawnPosition, "strange_portal", true, false, true);
				__instance.EndOfMonthStatus = "一个奇怪的入口出现了..";
				GameCamera.instance.TargetPositionOverride = new Vector3?(cardData3.transform.position);
				yield return new WaitForSeconds(2f);
				GameCamera.instance.TargetPositionOverride = null;
				__instance.ContinueButtonText = "啊哦";
				__instance.ShowContinueButton = true;
				while (!__instance.ContinueClicked)
				{
					yield return null;
				}
				__instance.ContinueClicked = false;
			}
			if (Random.value <= 0.1f && __instance.CurrentMonth >= 8 && __instance.CurrentMonth % 2 == 0)
			{
				Vector3 randomSpawnPosition2 = __instance.GetRandomSpawnPosition();
				CardData cardData4 = __instance.CreateCard(randomSpawnPosition2, "travelling_cart", true, false, true);
				__instance.EndOfMonthStatus = "一辆旅行马车出现了..";
				GameCamera.instance.TargetPositionOverride = new Vector3?(cardData4.transform.position);
				yield return new WaitForSeconds(2f);
				GameCamera.instance.TargetPositionOverride = null;
				__instance.ContinueButtonText = "好的";
				__instance.ShowContinueButton = true;
				while (!__instance.ContinueClicked)
				{
					yield return null;
				}
				__instance.ContinueClicked = false;
			}
			__instance.EndOfMonthStatus = "";
			__instance.ContinueButtonText = "开始新的月";
			__instance.ShowContinueButton = true;
			while (!__instance.ContinueClicked)
			{
				yield return null;
			}
			__instance.ContinueClicked = false;
			GameCanvas.instance.SetScreen(GameCanvas.instance.GameScreen);
			GameCamera.instance.TargetPositionOverride = null;
			__instance.currentAnimationRoutine = null;
			__instance.Save(true);
			yield break;
		}

		[HarmonyPostfix, HarmonyPatch(typeof(WorldManager), "BossFightCompleteRoutine")]
		public static void BossFightCompleteRoutinePatch(ref IEnumerator __result)
        {
			Debug.Log("测试输出:BossFightCompleteRoutine");
			__result = BossFightCompleteRoutine();
		}

		public static IEnumerator BossFightCompleteRoutine()
        {
			var __instance = WorldManager.instance;
			GameCanvas.instance.SetScreen(GameCanvas.instance.EndOfMonthScreen);
			Demon demon = __instance.GetCard<Demon>();
			GameCamera.instance.TargetPositionOverride = new Vector3?(demon.transform.position);
			__instance.EndOfMonthText = "你杀死了恶魔!";
			__instance.EndOfMonthStatus = "";
			__instance.ShowContinueButton = true;
			__instance.ContinueButtonText = "干得好!";
			while (!__instance.ContinueClicked)
			{
				yield return null;
			}
			__instance.ContinueClicked = false;
			__instance.ShowContinueButton = false;
			yield return new WaitForSeconds(1f);
			demon.MyGameCard.DestroyCard(true, true);
			yield return new WaitForSeconds(1f);
			__instance.EndOfMonthText = "你完成了主线任务!";
			__instance.EndOfMonthStatus = "可以继续游玩收集所有卡片!";
			__instance.ShowContinueButton = true;
			__instance.ContinueButtonText = "干得好!";
			while (!__instance.ContinueClicked)
			{
				yield return null;
			}
			__instance.ContinueClicked = false;
			__instance.ShowContinueButton = false;
			GameCanvas.instance.SetScreen(GameCanvas.instance.GameScreen);
			__instance.currentAnimation = null;
			GameCamera.instance.TargetPositionOverride = null;
			yield break;
		}

		[HarmonyPostfix, HarmonyPatch(typeof(WorldManager), "BossFightCoroutine")]
		public static void BossFightCoroutinePatch(ref IEnumerator __result)
        {
			Debug.Log("测试输出:BossFightCoroutine");
			__result = BossFightCoroutine();
		}

		public static IEnumerator BossFightCoroutine()
        {
			var __instance = WorldManager.instance;
			GameCanvas.instance.SetScreen(GameCanvas.instance.EndOfMonthScreen);
			Temple temple = __instance.GetCard<Temple>();
			CardData goblet = __instance.GetCard("goblet");
			GameCamera.instance.TargetPositionOverride = new Vector3?(temple.transform.position);
			__instance.EndOfMonthText = "金杯被带到寺庙里";
			__instance.EndOfMonthStatus = "";
			if (!__instance.CurrentRunOptions.IsPeacefulMode)
			{
				__instance.ContinueButtonText = "开始仪式";
			}
			else
			{
				__instance.ContinueButtonText = "做得好!";
			}
			__instance.ShowContinueButton = true;
			while (!__instance.ContinueClicked)
			{
				yield return null;
			}
			__instance.ContinueClicked = false;
			__instance.ShowContinueButton = false;
			__instance.EndOfMonthText = "";
			yield return new WaitForSeconds(0.5f);
			temple.MyGameCard.DestroyCard(true, true);
			goblet.MyGameCard.DestroyCard(true, true);
			if (!__instance.CurrentRunOptions.IsPeacefulMode)
			{
				__instance.CreateCard(temple.transform.position, "demon", true, false, true);
			}
			GameCamera.instance.TargetPositionOverride = null;
			GameCanvas.instance.SetScreen(GameCanvas.instance.GameScreen);
			__instance.currentAnimation = null;
			yield break;
		}

		[HarmonyPostfix, HarmonyPatch(typeof(WorldManager), "JustUnlockedPackCoroutine")]
		public static void JustUnlockedPackCoroutinePatch(Boosterpack justUnlockedPack, ref IEnumerator __result)
        {
			Debug.Log("测试输出:JustUnlockedPackCoroutine");
			__result = JustUnlockedPackCoroutine(justUnlockedPack);
		}

		public static IEnumerator JustUnlockedPackCoroutine(Boosterpack justUnlockedPack)
        {
			var __instance = WorldManager.instance;
			GameCanvas.instance.SetScreen(GameCanvas.instance.EndOfMonthScreen);
			if (justUnlockedPack != null)
			{
				BuyBoosterBox buyBoosterBox = Object.FindObjectsOfType<BuyBoosterBox>().FirstOrDefault((BuyBoosterBox x) => x.BoosterId == justUnlockedPack.BoosterId);
				GameCamera.instance.TargetPositionOverride = new Vector3?(buyBoosterBox.transform.position);
				__instance.EndOfMonthText = "新卡包解锁!";
				__instance.EndOfMonthStatus = justUnlockedPack.Name + "包现在可以使用了";
				__instance.ShowContinueButton = true;
				__instance.ContinueButtonText = "继续";
				while (!__instance.ContinueClicked)
				{
					yield return null;
				}
				__instance.ShowContinueButton = false;
				if (justUnlockedPack.BoosterId == "structures")
				{
					AchievementManager.instance.SpecialActionComplete("unlocked_all_packs", null);
				}
			}
			GameCamera.instance.TargetPositionOverride = null;
			GameCanvas.instance.SetScreen(GameCanvas.instance.GameScreen);
			__instance.currentAnimation = null;
			yield break;
		}
	}
}
