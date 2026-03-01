using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Leaf饰品
    /// </summary>
    internal class MorePlugins_Leaf : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Leaf";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Green; // 绿色稀有度
            Item.value = Item.sellPrice(gold: 3); // 售价3金币
        }
        #endregion

        #region 合成配方
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.JungleSpores, 10)        // 10个丛林孢子
                .AddIngredient(ItemID.Stinger, 8)             // 8个毒刺
                .AddIngredient(ItemID.Vine, 4)                // 4个藤蔓
                .AddTile(TileID.WorkBenches)                  // 工作台合成
                .Register();
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 魔力伤害提升3%
            player.GetDamage(DamageClass.Magic) += 0.03f;

            // 魔法暴击率提升5%
            player.GetCritChance(DamageClass.Magic) += 5f;

            // 法师武器面板伤害提升3点
            player.GetDamage(DamageClass.Generic).Flat += 3f;

            // 最大魔力值提升40
            player.statManaMax2 += 40;
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