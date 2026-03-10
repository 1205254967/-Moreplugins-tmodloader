using Moreplugins.Core.Utilities;
using Terraria;
using Terraria.ID;

namespace Moreplugins.Content.Items.Accessories
{
    internal class WoodPlugins : BasicPlugins
    {

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(copper: 10);

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.MPPlayer().woodPluginsEquipped = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("Wood", 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
