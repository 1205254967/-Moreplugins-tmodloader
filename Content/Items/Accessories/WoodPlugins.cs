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
            base.UpdateAccessory(player, hideVisual);
            woodPluginsTime++;
            if (woodPluginsTime < GetSeconds(30))
                return;
            if (Main.rand.NextBool(4))
                player.AddBuff(BuffID.Tipsy, GetSeconds(30));

            woodPluginsTime = 0;

        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.Wood, 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
