using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Steel饰品 - 合金饰品
    /// </summary>
    internal class SteelPlugins : BasicPlugins
    {

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Blue; // 蓝色稀有度
            Item.value = Item.sellPrice(silver: 80); // 售价80银币
        }

        public override void AddRecipes()
        {
            // 铁锭版本配方
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 5)       // 5个铁锭
                .AddTile(TileID.Anvils)                 // 铁砧/铅砧合成
                .Register();

            // 铅锭版本配方（兼容铅锭）
            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 5)       // 5个铅锭
                .AddTile(TileID.Anvils)                 // 铁砧/铅砧合成
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            // 伤害加成1%
            player.GetDamage(DamageClass.Generic) += 0.01f;

            // 护甲穿透一点
            player.GetArmorPenetration(DamageClass.Generic) += 1;

            // 攻击速度提升3%
            player.GetAttackSpeed(DamageClass.Generic) += 0.03f;

            // 防御力提升1点
            player.statDefense += 1;

            // 伤害减免1%
            player.endurance += 0.01f;

            // 移动速度提升5%
            player.moveSpeed += 0.05f;

            // 工具使用速度提升5%
            player.pickSpeed -= 0.05f; // 原版逻辑：数值越小速度越快
        }
    }

}