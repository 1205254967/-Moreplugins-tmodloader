using Microsoft.Xna.Framework;
using Moreplugins.Content.Players;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Massacre饰品
    /// </summary>
    internal class MassacrePlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Orange; // 橙色稀有度
            Item.value = Item.sellPrice(gold: 3); // 售价3金币
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
            // 获得1点生命再生
            player.lifeRegen += 1;

            // 增加2点防御力
            player.statDefense += 2;

            // 标记饰品已装备
            player.GetModPlayer<PluginsPlayer>().massacreEquipped = true;
        }
    }
}