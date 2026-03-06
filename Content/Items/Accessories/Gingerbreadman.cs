using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Moreplugins.Content.Players;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Moreplugins.Content.Items.Accessories
{
    public class GingerbreadmanPlugins : BasicPlugins
    {
        public override void SetStaticDefaults()
        {
            // 静态默认值设置
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Pink;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);

            // 为玩家添加姜饼人效果
            var modPlayer = player.GetModPlayer<GingerbreadmanPlayer>();
            modPlayer.hasGingerbreadman = true;
        }
    }
}

namespace Moreplugins.Content.Players
{
    public class GingerbreadmanPlayer : ModPlayer
    {
        public bool hasGingerbreadman = false;
        public bool hasUsedEffect = false;
        public int timer = 0;
        public int damagePercent = 10;
        public float damageTimer = 0;

        public override void ResetEffects()
        {
            // 只重置装备状态，不重置效果状态
            hasGingerbreadman = false;
        }

        public override void OnRespawn()
        {
            // 当玩家复活时，重置姜饼人效果
            hasUsedEffect = false;
            timer = 0;
            damagePercent = 10;
            damageTimer = 0;
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            // 检查玩家是否装备了姜饼人，并且还未使用过效果
            if (hasGingerbreadman && !hasUsedEffect)
            {
                // 立即回满血量
                Player.statLife = Player.statLifeMax2;
                Player.HealEffect(Player.statLifeMax2);

                // 标记已使用效果
                hasUsedEffect = true;
                timer = 0;
                damagePercent = 10;
                damageTimer = 0;

                return false;
            }
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
        }

        public override void PostUpdate()
        {
            // 如果已经使用了姜饼人效果，持续减少生命值
            if (hasUsedEffect)
            {
                timer++;
                damageTimer += 1f / 60f; // 每帧增加1/60秒

                // 计算每秒内逐渐减少的生命值
                float damagePercentage = damagePercent / 100f;
                float totalDamage = Player.statLifeMax2 * damagePercentage;
                float damagePerFrame = totalDamage / 60f; // 每秒60帧，每帧减少的伤害

                // 每帧减少生命值
                Player.statLife = (int)(Player.statLife - damagePerFrame);

                // 确保生命值不会低于0
                if (Player.statLife <= 0)
                {
                    Player.statLife = 0;
                    // 手动触发玩家死亡
                    Player.KillMe(PlayerDeathReason.LegacyDefault(), 1000, 0);
                }

                // 每秒增加伤害百分比
                if (timer % 60 == 0)
                {
                    damagePercent += 5;
                    damageTimer = 0;
                }
            }

            // 如果玩家死亡，重置姜饼人效果
            if (Player.dead)
            {
                hasUsedEffect = false;
                timer = 0;
                damagePercent = 10;
                damageTimer = 0;
            }

            // 如果玩家脱下饰品，重置姜饼人效果
            if (!hasGingerbreadman)
            {
                hasUsedEffect = false;
                timer = 0;
                damagePercent = 10;
                damageTimer = 0;
            }
        }
    }
}