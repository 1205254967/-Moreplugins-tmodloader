using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// NuclearWarhead饰品 - 双头核弹
    /// </summary>
    internal class NuclearWarheadPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Red; // 红色稀有度
            Item.value = Item.sellPrice(gold: 50); // 售价50金币
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ShroomiteBar, 10)      // 10个蘑菇矿锭
                .AddIngredient(ItemID.RocketI, 100)          // 100个火箭一型
                .AddIngredient(ItemID.RocketI, 100)          // 100个火箭一型
                .AddIngredient(ItemID.AdhesiveBandage, 1)     // 1个粘性绷带
                .AddTile(TileID.TinkerersWorkbench)         // 工匠作坊合成
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 标记饰品已装备
            base.UpdateAccessory(player, hideVisual);
            player.GetModPlayer<PluginsPlayer>().nuclearWarheadEquipped = true;
        }
    }
}