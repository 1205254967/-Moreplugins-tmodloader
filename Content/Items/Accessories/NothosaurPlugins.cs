using Terraria;
using Terraria.ID;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Nothosaur饰品 - 幻龙饰品
    /// </summary>
    internal class NothosaurPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Lime; // 绿色稀有度
            Item.value = Item.sellPrice(gold: 10); // 售价10金币
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            // 提升2最大仆从数
            player.maxMinions += 2;

            // 获得水上行走
            player.waterWalk = true;

            // 标记饰品已装备
            player.GetModPlayer<PluginsPlayer>().nothosaurEquipped = true;
        }
    }
}