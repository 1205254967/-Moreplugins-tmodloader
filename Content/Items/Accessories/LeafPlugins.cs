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
    internal class LeafPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Green; // 绿色稀有度
            Item.value = Item.sellPrice(gold: 3); // 售价3金币
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.JungleSpores, 10)        // 10个丛林孢子
                .AddIngredient(ItemID.Stinger, 8)             // 8个毒刺
                .AddIngredient(ItemID.Vine, 4)                // 4个藤蔓
                .AddTile(TileID.WorkBenches)                  // 工作台合成
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            // 魔力伤害提升3%
            player.GetDamage(DamageClass.Magic) += 0.03f;

            // 魔法暴击率提升5%
            player.GetCritChance(DamageClass.Magic) += 5f;

            // 法师武器面板伤害提升3点
            player.GetDamage(DamageClass.Generic).Flat += 3f;

            // 最大魔力值提升40
            player.statManaMax2 += 40;
        }
    }
}