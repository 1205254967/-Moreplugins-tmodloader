using Terraria;
using Terraria.ID;
using Moreplugins.Content.Players;
using Moreplugins.Core.Utilities;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// LavaSeed饰品
    /// </summary>
    public class LavaSeedPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 3);
            Item.defense = 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 10)        // 10个狱石锭
                .AddIngredient(ItemID.LavaBucket, 1)          // 1个岩浆桶
                .AddIngredient(ItemID.Fireblossom, 5)         // 5个火焰花
                .AddTile(TileID.Anvils)                      // 铁砧/铅砧合成
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.endurance += 0.05f;
            player.MPPlayer().lavaSeedEquipped = true;
        }
    }
}