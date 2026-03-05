using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Moreplugins.Content.Items.Accessories
{
    internal class MagicMirrorPlugins : BasicPlugins
    {
        int magicMirrorPluginsTime = 3600;
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 80);
            base.SetDefaults();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Vector2 spawnPosition = new Vector2(
            (player.SpawnX >= 0 ? player.SpawnX : Main.spawnTileX) * 16f,
            (player.SpawnY >= 0 ? player.SpawnY : Main.spawnTileY) * 16f);
            magicMirrorPluginsTime++;
            if (magicMirrorPluginsTime >= 3600 && player.statLife <= 30)
            {

                player.RemoveAllGrapplingHooks();
                player.Heal(30);
                player.Teleport(spawnPosition, TeleportationStyleID.RecallPotion);
                magicMirrorPluginsTime = 0;
            }
            base.UpdateAccessory(player, hideVisual);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Glass, 10)
                .AddIngredient(ItemID.IronBar, 8)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
