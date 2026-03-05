using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Moreplugins.Content.Items.Accessories
{
    internal class MushroomPlugins : BasicPlugins
    {
        int mushroomPluginsTime = 3600;
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(copper: 20);
            base.SetDefaults();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            mushroomPluginsTime++;
            if (mushroomPluginsTime >= 3600 && player.statLife <= 30)
            {
                SoundEngine.PlaySound(SoundID.Item2);
                player.Heal(70);
                mushroomPluginsTime = 0;
            }
            base.UpdateAccessory(player, hideVisual);
        }
    }
}
