using Terraria;
using Terraria.ID;
using Moreplugins.Content.Players;
using Moreplugins.Core.Utilities;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Nothosaur饰品 - 幻龙饰品
    /// </summary>
    public class NothosaurPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Lime; // 绿色稀有度
            Item.value = Item.sellPrice(gold: 10); // 售价10金币
            base.SetDefaults();    
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.maxMinions += 2;
            // 水上行走
            player.waterWalk = true;
            player.MPPlayer().nothosaurEquipped = true;
        }
    }
}