using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// TerraHeart饰品 - 泰拉之心
    /// </summary>
    internal class TerraHeart : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/TerraHeart";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Yellow; // 金色稀有度
            Item.value = Item.sellPrice(gold: 50); // 售价50金币
        }
        #endregion

        #region 合成配方
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MorePlugins_Dusk>())      // Dusk饰品
                .AddIngredient(ModContent.ItemType<Pure>())      // Pure饰品
                .AddIngredient(ItemID.DestroyerEmblem, 1) // 毁灭者徽章
                .AddTile(TileID.TinkerersWorkbench)               // 工匠作坊合成
                .Register();
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 提升10点武器基础面板伤害
            player.GetDamage(DamageClass.Generic).Flat += 5f;
            // 10点防御力
            player.statDefense += 10;
            // 10点伤害减免
            player.endurance += 0.05f;
            // 乘算伤害加成15%
            player.GetDamage(DamageClass.Generic) *= 1.10f;
            // 暴击率提升10%
            player.GetCritChance(DamageClass.Generic) += 8f;
            // 3点魔力再生
            player.manaRegen += 2;
            // 3点生命再生
            player.lifeRegen += 2;
            // 50点最大生命值
            player.statLifeMax2 += 40;
            // 50点最大魔力值
            player.statManaMax2 += 40;
            // 2最大仆从数
            player.maxMinions += 2;

            // 标记饰品已装备
            player.GetModPlayer<TerraHeartPlayer>().terraHeartEquipped = true;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion
    }

    /// <summary>
    /// TerraHeart饰品的玩家类
    /// </summary>
    public class TerraHeartPlayer : ModPlayer
    {
        public bool terraHeartEquipped; // 饰品是否装备
        public int attackTimer;
        public bool damageBoostActive;

        public override void ResetEffects()
        {
            terraHeartEquipped = false;
        }

        public override void PostUpdate()
        {
            if (terraHeartEquipped)
            {
                // 每过15秒，下次伤害提升500%
                attackTimer++;
                if (attackTimer >= 900) // 15秒 = 900帧
                {
                    damageBoostActive = true;
                }
            }
        }

        private bool isBoostedHit; // 标记是否是提升后的伤害

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            isBoostedHit = false;
            if (terraHeartEquipped)
            {
                // 造成着火了，霜冻，灵液减益5秒
                target.AddBuff(BuffID.OnFire, 300); // 5秒 = 300帧
                target.AddBuff(BuffID.Frostburn, 300);
                target.AddBuff(BuffID.Ichor, 300);

                // 每过15秒，下次伤害提升500%
                if (damageBoostActive)
                {
                    modifiers.SourceDamage *= 4f; // 500%提升
                    damageBoostActive = false;
                    attackTimer = 0;
                    isBoostedHit = true;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (terraHeartEquipped && isBoostedHit)
            {
                // 回复伤害的10%
                int healAmount = (int)(damageDone * 0.1f);
                if (healAmount > 0)
                {
                    Player.Heal(healAmount);
                }
            }
        }
    }
}