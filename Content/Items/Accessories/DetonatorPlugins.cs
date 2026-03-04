using Moreplugins.Content.Players;
using Terraria;
using Terraria.ID;

namespace Moreplugins.Content.Items.Accessories
{
    internal class DetonatorPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 15);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 标记饰品已装备
            base.UpdateAccessory(player, hideVisual);
            player.GetModPlayer<PluginsPlayer>().detonatorPluginsEquipped = true;
        }
    }
}