using Moreplugins.Content.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Moreplugins.Globals.Methods
{
    /// <summary>
    /// 需要看懂这里的话得去简单看一眼C#的知识：拓展方法
    /// </summary>
    public static partial class PluginMethods 
    {
        public static PluginsPlayer Plugins(this Player player) => player.GetModPlayer<PluginsPlayer>();
        /// <summary>
        /// 直接添加一个Loot，用最基础的common规则，重载moditem
        /// </summary>
        /// <param name="loot"></param>
        /// <param name="type">需要掉落的物品种类</param>
        /// <param name="chance">概率，这个数字是个分母，也就是概率为1/chance</param>
        /// <param name="minCount">最小掉落，默认1</param>
        /// <param name="maxCount">最大掉落,默认1</param>
        public static void AddLootCommon<T>(this NPCLoot loot, int chance, int minCount = 1, int maxCount = 1) where T : ModItem => AddLootCommon(loot, ItemType<T>(), chance, minCount, maxCount);
        /// <summary>
        /// 直接添加一个Loot，用最基础的common规则
        /// </summary>
        /// <param name="loot"></param>
        /// <param name="type">需要掉落的物品种类</param>
        /// <param name="chance">概率，这个数字是个分母，也就是概率为1/chance</param>
        /// <param name="minCount">最小掉落，默认1</param>
        /// <param name="maxCount">最大掉落,默认1</param>
        public static void AddLootCommon(this NPCLoot loot, int type, int chance, int minCount = 1, int maxCount = 1) => loot.Add(ItemDropRule.Common(type, chance, minCount, maxCount));
        public static string ToLanValue(this string langPath) => Language.GetTextValue(langPath);
    }
}
