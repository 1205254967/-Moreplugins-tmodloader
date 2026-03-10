using Moreplugins.Core.Utilities;
using Terraria;
using Terraria.ID;

namespace Moreplugins.Content.Items.Accessories
{
    public class GingerbreadmanPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Pink;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.MPPlayer().gingerbreadmanPluginsEquipped = true;
        }
    }
}