using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Moreplugins.Content.Players;
using Moreplugins.Core.Utilities;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Unity饰品 - 团结
    /// </summary>
    internal class UnityPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Red; // 红色稀有度
            Item.value = Item.sellPrice(gold: 100);
            Item.defense = 25;
            Item.manaIncrease = 100;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.MPPlayer().unityEquipped = true;
            player.maxMinions += 2;
            // 15%的护甲减免
            player.endurance += 0.15f;
            // 20%的暴击率
            player.GetCritChance(DamageClass.Generic) += 20f;
            // 30%的伤害加成
            player.GetDamage(DamageClass.Generic) += 0.3f;
            player.manaRegen += 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentNebula, 5)      // 5个星云碎片
                .AddIngredient(ItemID.FragmentVortex, 5)      // 5个星璇碎片
                .AddIngredient(ItemID.FragmentStardust, 5)    // 5个星尘碎片
                .AddIngredient(ItemID.FragmentSolar, 5)       // 5个日耀碎片
                .AddTile(TileID.LunarCraftingStation)         // 远古操纵机合成
                .Register();
        }
    }
}