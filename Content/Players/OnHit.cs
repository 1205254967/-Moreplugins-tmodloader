using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Content.Players
{
    public partial class PluginPlayer : ModPlayer
    {
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 检查是否是饰品召唤的激光击中敌人
            if (proj.active && proj.ai[1] == 99f)
            {
                // 根据激光类型添加不同的buff
                switch (proj.type)
                {
                    case ProjectileID.GreenLaser: // 绿色激光
                        target.AddBuff(BuffID.CursedInferno, 180); // 3秒诅咒狱火
                        break;
                    case ProjectileID.VortexLaser: // 黄色激光
                        target.AddBuff(BuffID.Ichor, 180); // 3秒灵液
                        break;
                    case ProjectileID.NebulaLaser: // 蓝色激光
                        target.AddBuff(BuffID.Frostburn, 180); // 3秒霜冻
                        break;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (ghostEquipped)
            {
                // 回复造成伤害的1%
                int healAmount = (int)(damageDone * 0.01f);
                if (healAmount > 0)
                {
                    Player.Heal(healAmount);
                }
            }

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
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            // 黄昏插件逻辑
            if (duskEquipped)
            {
                // 攻击使敌人受到着火了减益，持续5秒
                target.AddBuff(BuffID.OnFire, 300); // 5秒 = 300帧
                                                    // 攻击使敌人受到霜冻减益，持续5秒
                target.AddBuff(BuffID.Frostburn, 300); // 5秒 = 300帧
                                                       // 攻击使敌人受到灵液减益，持续5秒
                target.AddBuff(BuffID.Ichor, 300); // 5秒 = 300帧
            }

            // 村正插件逻辑
            if (damageBoostActive)
            {
                // 下一次攻击伤害提升50%
                modifiers.FinalDamage *= 1.5f;
                // 重置状态
                damageBoostActive = false;
                boostTimer = 300; // 5秒（60帧/秒 * 5秒）
            }

            // 熔岩之心插件逻辑
            if (lavaSeedEquipped)
            {
                // 攻击使敌人着火（原版减益）
                target.AddBuff(BuffID.OnFire, 600); // 10秒着火效果
            }

            // 血腥插件逻辑
            if (massacreEquipped)
            {
                // 攻击敌人后使敌人流血
                if (!bleedingNPCs.ContainsKey(target.whoAmI))
                {
                    bleedingNPCs[target.whoAmI] = 300; // 5秒（60帧/秒 * 5秒）
                }
                else
                {
                    // 重置计时器
                    bleedingNPCs[target.whoAmI] = 300;
                }
            }

            // 永夜插件逻辑
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

            // 腐化插件逻辑
            if (shadowyeggplantEquipped)
            {
                if (Main.rand.NextBool(10))
                {
                    modifiers.FinalDamage *= 2f;
                }
            }

            // 泰拉之心逻辑
            if (terraHeartEquipped)
            {
                isBoostedHit = false;
                // 造成着火了，霜冻，灵液减益5秒
                target.AddBuff(BuffID.OnFire, 300); // 5秒 = 300帧
                target.AddBuff(BuffID.Frostburn, 300);
                target.AddBuff(BuffID.Ichor, 300);

                // 每过15秒，下次伤害提升500%
                if (terraHeartAamageBoostActive)
                {
                    modifiers.SourceDamage *= 4f; // 500%提升
                    terraHeartAamageBoostActive = false;
                    terraHeartAttackTimer = 0;
                    isBoostedHit = true;
                }
            }
        }

    }
}
