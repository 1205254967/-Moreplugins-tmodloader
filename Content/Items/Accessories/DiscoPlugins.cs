using Terraria;
using Terraria.ID;
using Moreplugins.Content.Players;
namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Disco饰品 - 迪斯科棱晶
    /// </summary>
    internal class DiscoPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.GetModPlayer<PluginsPlayer>().discoEquipped = true;
            player.maxTurrets += 1; // 增加一哨兵栏位
        }
    }
}