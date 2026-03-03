using Terraria;
using Terraria.ModLoader;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    public abstract class BasicPlugins : ModItem, ILocalizedModType
    {
        /// <summary>
        /// 将物品的本地化路径重新导向到这个路径之下
        /// </summary>
        public new string LocalizationCategory => "Items.Accessories";
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (equippedItem.ModItem is BasicPlugins && incomingItem.ModItem is BasicPlugins)
            {
                return false;
            }
            return true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
    }
}