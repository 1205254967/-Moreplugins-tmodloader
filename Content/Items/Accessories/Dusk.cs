using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Moreplugins.Content.Players;


namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Dusk饰品 - 黄昏饰品
    /// </summary>
    internal class MorePlugins_Dusk : BasicPlugins

    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Dusk";

        #region 基础属性配置

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Yellow; // 黄色稀有度
            Item.value = Item.sellPrice(gold: 15); // 售价15金币
        }
        #endregion

        #region 合成配方
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MorePlugins_Night>(), 1)       // 1个夜晚
                .AddIngredient(ItemID.BrokenHeroSword, 1)                    // 1个断裂英雄剑
                .AddIngredient(ItemID.FrostCore, 1)                         // 1个寒霜核
                .AddIngredient(ItemID.Ichor, 10)                             // 10个灵液
                .AddTile(TileID.MythrilAnvil)                               // 秘银砧/山铜砧合成
                .Register();
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 提升5点武器基础面板伤害
            player.GetDamage(DamageClass.Generic).Flat += 5f;
            // 伤害提升5%
            player.GetDamage(DamageClass.Generic) += 0.05f;
            // 暴击率提升5%
            player.GetCritChance(DamageClass.Generic) += 5f;
            // 提升10%攻击速度
            player.GetAttackSpeed(DamageClass.Generic) += 0.1f;
            // 5防御力
            player.statDefense += 5;
            // 5%的伤害减免
            player.endurance += 0.05f;
            // 获得50点最大魔力值
            player.statManaMax2 += 50;
            // 2点魔力再生
            player.manaRegen += 2;
            // 2点生命再生
            player.lifeRegen += 2;
            // 免疫燃烧与着火了减益
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Burning] = true;

            // 标记饰品已装备
            player.GetModPlayer<DuskPlayer>().duskEquipped = true;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion

        #region 工具提示
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // 添加自定义提示文本
        }
        #endregion
    }

    /// <summary>
    /// Dusk饰品的玩家类
    /// </summary>
    public class DuskPlayer : ModPlayer
    {
        public bool duskEquipped; // 饰品是否装备

        public override void ResetEffects()
        {
            duskEquipped = false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (duskEquipped)
            {
                // 攻击使敌人受到着火了减益，持续5秒
                target.AddBuff(BuffID.OnFire, 300); // 5秒 = 300帧
                // 攻击使敌人受到霜冻减益，持续5秒
                target.AddBuff(BuffID.Frostburn, 300); // 5秒 = 300帧
                // 攻击使敌人受到灵液减益，持续5秒
                target.AddBuff(BuffID.Ichor, 300); // 5秒 = 300帧
            }
        }
    }
}