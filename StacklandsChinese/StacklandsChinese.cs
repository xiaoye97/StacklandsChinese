using TMPro;
using System;
using BepInEx;
using System.IO;
using HarmonyLib;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace StacklandsChinese
{
    [BepInPlugin("me.xiaoye97.plugin.Stacklands.StacklandsChinese", "Stacklands汉化模组", "1.1.0")]
    public class StacklandsChinese : BaseUnityPlugin
    {
        public static TMP_FontAsset TMPTranslateFont;
        public static Dictionary<string, string> I18NDict = new Dictionary<string, string>();

        void Start()
        {
            LoadFont();
            LoadChinese();
            I18NEnums();
            Harmony.CreateAndPatchAll(typeof(I18NAchievementManager));
            Harmony.CreateAndPatchAll(typeof(I18NFont));
            Harmony.CreateAndPatchAll(typeof(I18NWorldManager));
            Harmony.CreateAndPatchAll(typeof(I18NMainMenu));
            Harmony.CreateAndPatchAll(typeof(I18NOptionsScreen));
            Harmony.CreateAndPatchAll(typeof(I18NSelectResolutionScreen));
            Harmony.CreateAndPatchAll(typeof(I18NCreditsScreen));
            Harmony.CreateAndPatchAll(typeof(I18NCardopediaScreen));
            Harmony.CreateAndPatchAll(typeof(I18NGameCanvas));
            Harmony.CreateAndPatchAll(typeof(I18NRunOptionsScreen));
            Harmony.CreateAndPatchAll(typeof(I18NPauseScreen));
            Harmony.CreateAndPatchAll(typeof(I18NGameScreen));
            Harmony.CreateAndPatchAll(typeof(I18NSellBox));
            Harmony.CreateAndPatchAll(typeof(I18NMisc));
        }

        public void I18NEnums()
        {
            SpawnEnumI18N(typeof(AchievementGroup));
            SpawnEnumI18N(typeof(CardType));
            SpawnEnumI18N(typeof(BlueprintGroup));
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                SaveChinese();
            }
        }

        /// <summary>
        /// 加载字体
        /// </summary>
        public void LoadFont()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("StacklandsChinese.font");
            AssetBundle ab = AssetBundle.LoadFromStream(stream);
            TMPTranslateFont = ab.LoadAsset<TMP_FontAsset>("SIMHEI SDF");
        }

        /// <summary>
        /// 加载文本
        /// </summary>
        public void LoadChinese()
        {
            string path = $"{Paths.ConfigPath}/Chinese.txt";
            if (File.Exists(path))
            {
                var lines = File.ReadAllLines(path);
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var args = line.Split(new char[] { '=' }, 2);
                        if (args.Length == 2)
                        {
                            I18NDict[args[0]] = args[1].Replace("\\n", "\n");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 保存文本
        /// </summary>
        public void SaveChinese()
        {
            string path = $"{Paths.ConfigPath}/Chinese.txt";
            StringBuilder sb = new StringBuilder();
            foreach (var kv in I18NDict)
            {
                if (!string.IsNullOrWhiteSpace(kv.Value))
                {
                    string line = $"{kv.Key}={kv.Value.Replace("\n", "\\n")}";
                    sb.AppendLine(line);
                }
            }
            File.WriteAllText(path, sb.ToString());
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static string GetI18N(string key, string defValue)
        {
            if (I18NDict.ContainsKey(key))
            {
                return I18NDict[key];
            }
            else
            {
                I18NDict[key] = defValue;
                if (!string.IsNullOrWhiteSpace(defValue))
                {
                    Debug.Log($"{key}={defValue}");
                }
                return defValue;
            }
        }

        /// <summary>
        /// 生成枚举的翻译
        /// </summary>
        /// <param name="type"></param>
        public static void SpawnEnumI18N(Type type)
        {
            var names = Enum.GetNames(type);
            var enumName = type.Name;
            foreach (var ename in names)
            {
                GetI18N($"{enumName}_{ename}", ename);
            }
        }

        /// <summary>
        /// 在IL中替换文本
        /// </summary>
        public static IEnumerable<CodeInstruction> ReplaceIL(IEnumerable<CodeInstruction> instructions, string target, string i18n)
        {
            var list = instructions.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var ci = list[i];
                if (ci.opcode == OpCodes.Ldstr)
                {
                    if ((string)ci.operand == target)
                    {
                        ci.operand = i18n;
                    }
                }
            }
            return list.AsEnumerable();
        }
    }
}
