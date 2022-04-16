using HarmonyLib;

namespace StacklandsChinese
{
    public static class I18NAchievementManager
    {
        [HarmonyPostfix, HarmonyPatch(typeof(AchievementManager), "Awake")]
        public static void AwakeParch(AchievementManager __instance)
        {
            foreach (var achievement in __instance.AllAchievements)
            {
                achievement.Description = StacklandsChinese.GetI18N($"AchievementDescription_{achievement.Id}", achievement.Description);
            }
        }
    }
}
