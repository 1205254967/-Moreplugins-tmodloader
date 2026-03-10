using Microsoft.Xna.Framework;
using Moreplugins.Content.Players;
using Moreplugins.Core.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Massacre饰品
    /// </summary>
    public class MassacrePlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 3);
            Item.lifeRegen = 1;
            Item.defense = 2;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 5)        // 5个猩红锭
                .AddIngredient(ItemID.TissueSample, 5)       // 5个组织样本
                .AddTile(TileID.Anvils)                      // 铁砧/铅砧合成
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.MPPlayer().massacreEquipped = true;
        }
    }
}