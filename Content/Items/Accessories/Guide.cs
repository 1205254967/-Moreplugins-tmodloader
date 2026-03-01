using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Guide饰品
    /// </summary>
    internal class MorePlugins_Guide : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Guide";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Orange; // 橙色稀有度
            Item.value = Item.sellPrice(gold: 1); // 售价1金币
        }
        #endregion

        #region 合成配方（可选）
        public override void AddRecipes()
        {
            // 可以添加合成配方，或者只作为掉落物品
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 获得20点最大生命值
            player.statLifeMax2 += 20;

            // 武器伤害直接在面板上增加3点
            player.GetDamage(DamageClass.Generic).Flat += 3f;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion

        #region 工具提示
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // 添加自定义提示文本
        }
        #endregion
    }
}