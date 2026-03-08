using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Content.Players;
using Moreplugins.Core.Utilities;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Holy饰品 - 神圣饰品
    /// </summary>
    public class HolyPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink; // 粉色稀有度
            Item.value = Item.sellPrice(gold: 5); // 售价5金币
            base.SetDefaults();    
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 3)       // 3个神圣锭
                .AddIngredient(ItemID.Ruby, 1)              // 1个红宝石
                .AddTile(TileID.MythrilAnvil)               // 秘银砧/山铜砧合成
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            // 3点防御
            player.statDefense += 3;
            // 1最大仆从数
            player.maxMinions += 1;
            // 召唤物有30%概率造成150%伤害
            //Scarlet: ↑改写成了一个独立的召唤物伤害机制
            //目前是直接赋值而不是加减，因为我也不知道你们要不要考虑叠加功能
            player.MPPlayer().summonCritChance = 0.3f;
            player.MPPlayer().summonCritDamageMultipler = 1.5f;
            // 5点护甲穿透
            player.GetArmorPenetration(DamageClass.Summon) += 5;
        }
    }
}