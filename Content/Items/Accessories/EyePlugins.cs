using Terraria;
using Terraria.ID;

namespace Moreplugins.Content.Items.Accessories
{
    internal class EyePlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.AddBuff(BuffID.NightOwl, 2);
            Lighting.AddLight(player.Center, TorchID.UltraBright);
        }
    }
}
