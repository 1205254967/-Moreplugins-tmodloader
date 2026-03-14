using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moreplugins.Content.Particles;
using Moreplugins.Core.Graphics.ParticleSystem;
using Moreplugins.Core.SystemLoader;
using System;
using Terraria;
using Terraria.Audio;
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
                UpdateTerraHeartModifyHit(target, ref modifiers);

            if (holyPluginsEquipped)
            {
                if (modifiers.DamageType == DamageClass.Summon && Main.rand.NextBool(3))
                {
                    SoundEngine.PlaySound(SoundID.Item105 with {MaxInstances = 0}, target.Center);
                    modifiers.SourceDamage *= 1.5f;
                    UpdateHolyParticle(target.Center);
                }
            }

            if (nightEquipped)
            {
                if (modifiers.DamageType == DamageClass.Summon && Main.rand.NextBool(3))
                {
                    modifiers.SourceDamage *= 1.5f;
                }
            }
        }
        /// <summary>
        /// 更加复杂化的粒子案例。这里会综合需要考研你对向量的理解
        /// </summary>
        /// <param name="center"></param>
        private void UpdateHolyParticle(Vector2 center)
        {
            for (int i = 0; i < 4; i++)
            {
                //单位向量，然后RotateBy旋转，会让这个向量覆盖四个方向。
                //这样我们就得到了"一个"允许你发射十字方向的单位向量
                Vector2 dir = Vector2.UnitX.RotatedBy(-PiOver2 * i);
                //而后我们基于这此实际生成需要的粒子
                for (int j = 0; j < 20; j++)
                {
                    //这里会给向上和向下的特判
                    //因为我们想做十字架效果，而十字架中上半部分较短下半部分较长
                    //因此在代码里我们也需要同样这样进行
                    //基准生成位置
                    Vector2 spawnPos = center + Main.rand.NextVector2Circular(4f, 4f);
                    //dir已经是基准的方向向量，我们提供一个模长即可
                    //这里的模长是故意在负数与正数随机的，因为我们需要确保中心点在视觉效果上“连接”起来
                    //具体可以在游戏内进行查看
                    Vector2 vel = dir * Main.rand.NextFloat(-1f, 5.8f);
                    //进行特判，等于1时为向上的方向。
                    //重新进行一次赋值
                    if (i == 1)
                        vel = dir * Main.rand.NextFloat(-1f, 3.8f);
                    //等于3则为朝下的方向，这里在进行一次复制
                    if (i == 3)
                        vel = dir * Main.rand.NextFloat(-1f, 7.8f);
                    //实际生产粒子
                    MPSystemLoader.SpawnParticle(new StarShape(spawnPos, vel * Main.rand.NextFloat(0.75f, 1.2f), Color.Gold, 0.8f, 40));
                    //只用上面的粒子会过于单调，我们需要补充一点宝石的红色，之类的
                    //随机在1/2概率生成，这样可以减少一点生成数量
                    //并且我们需要让这里的粒子在上面粒子的图层下方，这样可以让上面的粒子盖住他，让主题的十字架显露出来
                    if (Main.rand.NextBool(4))
                        MPSystemLoader.SpawnParticle(new ShinyOrbParticle(spawnPos, vel * 1.1f, Color.Crimson, 40, 0.8f, false), true);
                    //而后我们需要一点原版的神圣粒子做点缀。
                    if (Main.rand.NextBool(3))
                    {
                        Dust d = Dust.NewDustPerfect(spawnPos, DustID.GoldCoin, vel * Main.rand.NextFloat(0.7f, 0.9f));
                        d.scale *= 1.1f;
                    }
                }

            }
        }

        //泰拉之心
        private void UpdateTerraHeartModifyHit(NPC target, ref NPC.HitModifiers modifiers)
        {
            isBoostedHit = false;
            // 造成着火了，霜冻，灵液减益5秒
            target.AddBuff(BuffID.OnFire3, 300); // 5秒 = 300帧
            target.AddBuff(BuffID.Frostburn2, 300);
            target.AddBuff(BuffID.Ichor, 300);
            // 每过15秒，下次伤害提升500%
            if (terraHeartAamageBoostActive)
            {
                modifiers.SourceDamage *= 4f; // 500%提升
                terraHeartAamageBoostActive = false;
                terraHeartAttackTimer = 0;
                isBoostedHit = true;
                SoundEngine.PlaySound(SoundID.Item38, target.Center);
                UpdateTerraParticle(target.Center);
            }

        }

        private void UpdateTerraParticle(Vector2 center)
        {
            for (int i = 0; i < 24; i++)
            {
                //基点坐标+一个半径为4的圆范围内随机
                Vector2 spawnPos = center + Main.rand.NextVector2Circular(4, 4);
                //随机取2pi弧度，然后弧度转为单位向量后，再随机取模长（2~7.8)
                Vector2 vel = Main.rand.NextFloat(TwoPi).ToRotationVector2() * Main.rand.NextFloat(2, 7.8f);
                //实际创建粒子
                Dust d = Dust.NewDustPerfect(spawnPos, DustID.Terra, vel);
                d.scale *= Main.rand.NextFloat(1.2f, 1.6f);
                d.noGravity = true;
            }
            for (int i = 0; i < 22;i++)
            {
                //基点坐标+一个半径为6的圆范围内随机
                Vector2 spawnPos = center + Main.rand.NextVector2Circular(6, 6);
                //随机取2pi弧度，然后弧度转为单位向量后，再随机取模长（2~6.8)
                Vector2 vel = Main.rand.NextFloat(TwoPi).ToRotationVector2() * Main.rand.NextFloat(2, 6.8f);
                /*
                开始调用模组内自定义的粒子系统
                可以选择两种写法，第一种是先实例化粒子，然后再把它扔进粒子系统内的生成方式
                第二种就是在生成的时候直接实例化
                实际上两种都没区别，第一种和第二种本质上是一个写法上的取舍
                第一种写法如下
                StarShape starShape = new(spawnPos, vel, Color.Lerp(Color.Lime, Color.LimeGreen, Main.rand.NextFloat()), Main.rand.NextFloat(0.7f, 1.1f), 40);
                MPSystemLoader.SpawnParticle(starShape);
                第二种写法则如下，这里调用也是用的第二种写法
                */
                MPSystemLoader.SpawnParticle(new StarShape(spawnPos, vel, Color.Lerp(Color.Lime, Color.LimeGreen, Main.rand.NextFloat()), Main.rand.NextFloat(0.7f, 1.1f), 40));
            }
            for (int i = 0; i < 36; i++)
            {
                //同上。
                Vector2 spawnPos = center + Main.rand.NextVector2Circular(4, 4);
                Vector2 vel = Main.rand.NextFloat(TwoPi).ToRotationVector2() * Main.rand.NextFloat(2, 6.8f);
                MPSystemLoader.SpawnParticle(new ShinyOrbParticle(spawnPos, vel, Color.Lerp(Color.Lime, Color.Green, Main.rand.NextFloat()), 40, 0.8f, false));
            }
            MPSystemLoader.SpawnParticle(new BloomCircle(center, Vector2.Zero, Color.GreenYellow, 40, 0.48f));
        }
    }
}
