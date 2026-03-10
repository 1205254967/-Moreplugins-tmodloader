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
    public class LeafPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Green; // 绿色稀有度
            Item.value = Item.sellPrice(gold: 3); // 售价3金币
            Item.manaIncrease = 40;
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
            player.GetDamage(DamageClass.Magic) += 3 / 100f;
            player.GetCritChance(DamageClass.Magic) += 5f;
            player.GetDamage(DamageClass.Generic).Flat += 3f;
        }
    }
}