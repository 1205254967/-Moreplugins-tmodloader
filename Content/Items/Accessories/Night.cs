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
    /// Night饰品 - 夜晚饰品
    /// </summary>
    internal class MorePlugins_Night : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Night";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Orange; // 橙色稀有度
            Item.value = Item.sellPrice(gold: 5); // 售价5金币
        }
        #endregion

        #region 合成配方
        public override void AddRecipes()
        {
            // 第一个合成配方：使用屠戮
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MorePlugins_Leaf>(), 1)       // 1个树叶
                .AddIngredient(ModContent.ItemType<MorePlugins_LavaSeed>(), 1)    // 1个熔岩之心
                .AddIngredient(ModContent.ItemType<MorePlugins_Kaishakunin>(), 1) // 1个刽子手
                .AddIngredient(ModContent.ItemType<MorePlugins_Massacre>(), 1)    // 1个屠戮
                .AddTile(TileID.Anvils)                                         // 铁砧/铅砧合成
                .Register();

            // 第二个合成配方：使用阴暗的茄子
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MorePlugins_Leaf>(), 1)       // 1个树叶
                .AddIngredient(ModContent.ItemType<MorePlugins_LavaSeed>(), 1)    // 1个熔岩之心
                .AddIngredient(ModContent.ItemType<MorePlugins_Kaishakunin>(), 1) // 1个刽子手
                .AddIngredient(ModContent.ItemType<MorePlugins_Shadowyeggplant>(), 1) // 1个阴暗的茄子
                .AddTile(TileID.Anvils)                                         // 铁砧/铅砧合成
                .Register();
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 提升2点基础面板伤害
            player.GetDamage(DamageClass.Generic).Flat += 2f;
            // 3%伤害加成
            player.GetDamage(DamageClass.Generic) += 0.03f;
            // 暴击率提升3%
            player.GetCritChance(DamageClass.Generic) += 3f;
            // 获得3点防御
            player.statDefense += 3;
            // 3%伤害减免
            player.endurance += 0.03f;
            // 召唤物有15%概率造成150%伤害
            player.GetDamage(DamageClass.Summon) += 0.5f;
            // 最大魔力值提升30
            player.statManaMax2 += 30;
            // 免疫燃烧与着火了减益
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Burning] = true;

            // 标记饰品已装备
            player.GetModPlayer<NightPlayer>().nightEquipped = true;
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
    /// Night饰品的玩家类
    /// </summary>
    public class NightPlayer : ModPlayer
    {
        public bool nightEquipped; // 饰品是否装备
        private int attackTimer;

        public override void ResetEffects()
        {
            nightEquipped = false;
        }

        public override void PostUpdate()
        {
            if (nightEquipped)
            {
                // 每过五秒，下次伤害提升40%
                attackTimer++;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (nightEquipped)
            {
                // 攻击造成“着火了”减益，持续4秒
                target.AddBuff(BuffID.OnFire, 120); // 4秒 = 120帧

                // 每过五秒，下次伤害提升40%
                if (attackTimer >= 300) // 5秒 = 300帧
                {
                    modifiers.SourceDamage *= 1.4f;
                    attackTimer = 0;
                }
            }
        }
    }
}