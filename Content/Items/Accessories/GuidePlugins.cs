using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Guide饰品
    /// </summary>
    internal class GuidePlugins : BasicPlugins
    {

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Orange; // 橙色稀有度
            Item.value = Item.sellPrice(gold: 1); // 售价1金币
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            // 获得20点最大生命值
            player.statLifeMax2 += 20;

            // 武器伤害直接在面板上增加3点
            player.GetDamage(DamageClass.Generic).Flat += 3f;
        }
    }
}