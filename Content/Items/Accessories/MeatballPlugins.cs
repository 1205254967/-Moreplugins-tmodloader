using Moreplugins.Content.Players;
using Terraria;
using Terraria.ID;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Meatball饰品 - 肉球
    /// </summary>
    internal class MeatballPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(gold: 30);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.GetModPlayer<PluginsPlayer>().meatballEquipped = true;
        }
    }
}
