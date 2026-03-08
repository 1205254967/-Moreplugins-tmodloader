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
            Item.rare = ItemRarityID.Green; // 绿色稀有度
            Item.value = Item.sellPrice(gold: 3); // 售价3金币
            base.SetDefaults();    
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
            player.GetDamage<MagicDamageClass>() += 0.03f;
            player.GetCritChance<MagicDamageClass>() += 5f;
            player.GetDamage<MagicDamageClass>().Flat += 3;
            player.statManaMax2 += 40;
        }
    }
}