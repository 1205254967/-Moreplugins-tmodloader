using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Vibrissa饰品 - 触须
    /// </summary>
    internal class VibrissaPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Lime; // 绿色稀有度
            Item.value = Item.sellPrice(gold: 10); // 售价10金币
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BeeWax, 20)          // 20个蜂蜡
                .AddTile(TileID.TinkerersWorkbench)         // 工匠作坊合成
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.GetModPlayer<PluginsPlayer>().vibrissaEquipped = true;
        }
    }
}