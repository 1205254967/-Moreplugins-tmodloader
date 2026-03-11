using Microsoft.Xna.Framework;
using Moreplugins.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Content.Players
{
    public partial class PluginPlayer : ModPlayer
    {
        public override void PostUpdateMiscEffects()
        {
            if (soundAcc != soundAccOld)
            {
                SoundEngine.PlaySound(DefaultSound);
                soundAccOld = soundAcc;
            }

            if (hasUsedEffect)
            {
                dieTimer++;
                if (dieTimer % 60 == 0) { Player.statLife -= dieTimer / 60; }
                if( Player.statLife <=0) { Player.KillMe(PlayerDeathReason.LegacyDefault(), 1, 0); }
            }

            if (woodPluginsEquipped)
            {
                woodPluginsTime++;
                if (woodPluginsTime >= 1800)
                if (Main.rand.NextBool(4))
                {
                    Player.AddBuff(BuffID.Tipsy, 1800);
                    woodPluginsTime = 0;
                }
                else
                {
                    woodPluginsTime = 0;
                }
            }
        }
        public void BudPluginsEffect()
        {
            // 世纪之花插件逻辑
            timer++;
            //不符合需求的条件时提前return回去
            //尽量减少点嵌套
            if (timer < GetSeconds(10))
                return;
            Player.Heal(80);

            // 产生绿色粒子
            for (int i = 0; i < 20; i++)
            {
                Vector2 position = Player.Center + new Vector2(Main.rand.Next(-30, 31), Main.rand.Next(-30, 31));
                Vector2 velocity = new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
                Dust dust = Dust.NewDustDirect(position, 10, 10, DustID.JungleSpore, velocity.X, velocity.Y, 100, default, 1.5f);
                dust.noGravity = true;
            }
            // 产生两个孢子囊弹幕
            SpawnSporePods();

            timer = 0;
        }
        public void DiscoEffect()
        {
            // 检查七彩水晶是否存在
            //xbzz你tm不判宿主是吧
            if (!Player.HasProj(ProjectileID.RainbowCrystal))
                SpawnCrystal();
            else if (crystalIndex != -1)
            {
                // 让水晶跟随玩家移动
                Projectile crystal = Main.projectile[crystalIndex];
                Vector2 targetPosition = Player.Center + new Vector2(0, -100); // 在玩家头顶
                                                                               //卧槽不要用position用center啊哥哥
                crystal.Center = Vector2.Lerp(crystal.Center, targetPosition, 0.1f);
                crystal.netUpdate = true;
            }
        }
        public override void PostUpdate()
        {
            if (budEquipped)
                BudPluginsEffect();

            //迪斯科插件逻辑
            if (discoEquipped)
                DiscoEffect();
            else
            {
                // 如果饰品未装备，移除水晶
                if (crystalIndex != -1 && Main.projectile[crystalIndex].active)
                {
                    Main.projectile[crystalIndex].Kill();
                    crystalIndex = -1;
                }
            }

            // 石巨人之拳插件逻辑
            if (giantfistEquipped)
            {
                punchTimer++;
                if (punchTimer >= GetSeconds(60)) // 1分钟 = 3600帧
                {
                    PunchEnemy();
                    punchTimer = 0;
                }
            }

            // 村正插件逻辑
            if (kaishakuninEquipped)
            {
                if (boostTimer > 0)
                {
                    boostTimer--;
                }
                else
                {
                    // 每5秒激活一次伤害提升
                    damageBoostActive = true;
                }
            }

            // 血腥插件逻辑
            if (massacreEquipped)
            {
                // 处理流血效果
                List<int> npcsToRemove = new List<int>();

                foreach (var kvp in bleedingNPCs)
                {
                    int npcIndex = kvp.Key;
                    int timer = kvp.Value;

                    // 每秒钟造成4点伤害
                    if (Main.GameUpdateCount % 60 == 0)
                    {
                        NPC npc = Main.npc[npcIndex];
                        if (npc.active && !npc.dontTakeDamage)
                        {
                            NPC.HitInfo hitInfo = new NPC.HitInfo();
                            hitInfo.Damage = 10;
                            npc.StrikeNPC(hitInfo);

                            // 生成红色粒子效果
                            for (int i = 0; i < 3; i++)
                            {
                                Vector2 offset = new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-20, 21));
                                Vector2 position = npc.Center + offset;
                                Vector2 velocity = new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
                                Dust dust = Dust.NewDustPerfect(position, DustID.Blood, velocity, 100, new Color(255, 0, 0), 1.5f);
                                dust.noGravity = true;
                            }
                        }
                        else
                        {
                            npcsToRemove.Add(npcIndex);
                        }
                    }

                    // 减少计时器
                    timer--;
                    if (timer <= 0)
                    {
                        npcsToRemove.Add(npcIndex);
                    }
                    else
                    {
                        bleedingNPCs[npcIndex] = timer;
                    }
                }

                // 移除已经结束流血的NPC
                foreach (int npcIndex in npcsToRemove)
                {
                    bleedingNPCs.Remove(npcIndex);
                }
            }

            // 肉球插件逻辑
            if (meatballEquipped)
            {
                thornTimer++;
                if (thornTimer >= 240) // 2秒 = 120帧
                {
                    SpawnBloodThornProjectiles();
                    thornTimer = 0;
                }
            }

            // 永夜插件逻辑
            if (nightEquipped)
            {
                // 每过五秒，下次伤害提升40%
                attackTimer++;
            }

            // 幻龙插件逻辑
            if (nothosaurEquipped)
            {
                // 每过两秒发射一个爆炸泡泡
                bubbleTimer++;
                if (bubbleTimer >= 60) // 2秒 = 120帧
                {
                    SpawnBubble();
                    bubbleTimer = 0;
                }
            }

            // 核弹插件逻辑
            if (nuclearWarheadEquipped)
            {
                nukeTimer++;
                if (nukeTimer >= 180)//30秒
                {
                    SpawnFirstNuke();
                    nukeTimer = 0;
                }
            }

            // 户外生存装置插件逻辑
            if (kitEquipped)
            {
                // 每60帧检查一次是否需要召唤双子魔眼
                eyeTimer++;
                if (eyeTimer >= 60)
                {
                    SpawnTwins();
                    eyeTimer = 0;
                }

                // 每2秒随机发射1种不同颜色的激光
                laserTimer++;
                if (laserTimer >= 60)
                {
                    SpawnLaser();
                    laserTimer = 0;
                }
            }
            else
            {
                // 如果饰品未装备，移除所有饰品召唤的双子魔眼
                if (spazmaminiProjectileId != -1 && Main.projectile[spazmaminiProjectileId].active)
                {
                    Main.projectile[spazmaminiProjectileId].Kill();
                }
                if (retaniminiProjectileId != -1 && Main.projectile[retaniminiProjectileId].active)
                {
                    Main.projectile[retaniminiProjectileId].Kill();
                }
                spazmaminiProjectileId = -1;
                retaniminiProjectileId = -1;
            }

            // 萨满插件逻辑
            if (shamanEquipped)
            {
                samanAttackTimer++;
                if (attackTimer >= 60) // 1秒 = 60帧
                {
                    AttackEnemiesWithTentacles();
                    samanAttackTimer = 0;
                }
            }

            // 团结插件逻辑
            if (unityEquipped)
            {
                // 检查并召唤三个星辰细胞
                for (int i = 0; i < 3; i++)
                {
                    if (starCellProjectileIds[i] == -1 || !Main.projectile[starCellProjectileIds[i]].active)
                    {
                        starCellProjectileIds[i] = SpawnStarCell();
                    }
                }
            }
            else
            {
                // 如果饰品未装备，移除所有召唤物
                for (int i = 0; i < 3; i++)
                {
                    if (starCellProjectileIds[i] != -1 && Main.projectile[starCellProjectileIds[i]].active)
                    {
                        Main.projectile[starCellProjectileIds[i]].Kill();
                    }
                    starCellProjectileIds[i] = -1;
                }
            }

            // 泰拉之心插件逻辑
            if (terraHeartEquipped)
            {
                // 每过15秒，下次伤害提升500%
                terraHeartAttackTimer++;
                if (terraHeartAttackTimer >= 900) // 15秒 = 900帧
                {
                    terraHeartAamageBoostActive = true;
                }
            }

            // 黄蜂插件逻辑
            if (vibrissaEquipped)
            {
                // 每60帧检查一次是否需要召唤黄蜂
                beeTimer++;
                if (beeTimer >= 60)
                {
                    SpawnHornets();
                    beeTimer = 0;
                }
            }
            else
            {
                // 如果饰品未装备，移除所有饰品召唤的黄蜂
                if (hornetProjectileId1 != -1 && Main.projectile[hornetProjectileId1].active)
                {
                    Main.projectile[hornetProjectileId1].Kill();
                }
                if (hornetProjectileId2 != -1 && Main.projectile[hornetProjectileId2].active)
                {
                    Main.projectile[hornetProjectileId2].Kill();
                }
                hornetProjectileId1 = -1;
                hornetProjectileId2 = -1;
            }
        }

    }
}
