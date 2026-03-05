using Terraria;
using Terraria.ID;

namespace Moreplugins.Content.Items.Accessories
{
    internal class WoodPlugins : BasicPlugins
    {
        int woodPluginsTime = 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(copper: 10);

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            woodPluginsTime++;
            if(woodPluginsTime >= 1)
                if(Main.rand.Next(100) <= 24)
                {
                    player.AddBuff(BuffID.Tipsy, 1800);
                    woodPluginsTime = 0;
                }
                else
                {
                    woodPluginsTime = 0;
                }

            base.UpdateAccessory(player, hideVisual);
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
