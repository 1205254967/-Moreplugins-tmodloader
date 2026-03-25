using Moreplugins.Content.Dashs;
using Moreplugins.Content.Players.DashPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Content.Items.Accessories
{
    internal class SwordfishPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(silver: 70);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.GetModPlayer<DashPlayerManager>().CurDashID = GetInstance<SwordfishDash>().Type;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Swordfish, 1)
                .AddTile(TileID.Sawmill)
                .Register();

            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<SwordfishPlugins>()] = ItemID.Swordfish;
        }
    }
}