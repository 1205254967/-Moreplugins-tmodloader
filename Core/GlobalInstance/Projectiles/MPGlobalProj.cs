using Microsoft.Xna.Framework;
using Moreplugins.Content.Items.Accessories;
using Moreplugins.Content.Players;
using Moreplugins.Core.Utilities;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Core.GlobalInstance.Projectiles
{
    public partial class MPGlobalProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            // 检查是否是彩虹水晶发射的射弹
            if (projectile.type == ProjectileID.RainbowCrystalExplosion)
            {
                // 检查射弹的所有者是否装备了迪斯科棱晶
                Player player = Main.player[projectile.owner];
                if (player.active)
                {
                    PluginPlayer modPlayer = player.MPPlayer();
                    if (modPlayer.discoEquipped)
                    {
                        // 设置射弹伤害为500
                        projectile.damage = 500;
                        projectile.netUpdate = true;
                    }
                }
            }
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            Player owner = Main.player[projectile.owner];
            PluginPlayer modPlayer = owner.MPPlayer();
            if ((projectile.minion || projectile.sentry) && modPlayer.holyPlugin)
            {
                ModifySummonHit(owner, modPlayer, target, ref modifiers);
            }
        }

        private void ModifySummonHit(Player owner, PluginPlayer modPlayer, NPC target, ref NPC.HitModifiers modifiers)
        {
            float summonCritChance = modPlayer.summonCritChance;
            if(summonCritChance > 0)
            {
                if(Main.rand.NextFloat() < summonCritChance || summonCritChance >= 1f)
                    modifiers.SourceDamage *= modPlayer.summonCritDamageMultipler;
            }
        }

        //Scarlet: 这写法必定出事但是他妈的这段AI码也太他妈逆天了吧有时间再改吧
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 检查是否是核弹饰品发射的导弹（通过ai[2]标记）

            if (projectile.ai[2] == 1f)
            {
                // 获取发射导弹的玩家
                Player player = Main.player[projectile.owner];
                if (player.active)
                {
                    // 检查玩家是否装备了核弹饰品
                    PluginPlayer modPlayer = player.MPPlayer();
                    if (modPlayer.nuclearWarheadEquipped)
                    {
                        // 生成爆炸效果，扩大攻击范围（增加30%）
                        int explosionRadius = 390; // 爆炸半径，覆盖三个假人的距离，增加30%

                        // 计算带有远程伤害加成的爆炸伤害（提升至4000）
                        float baseDamage = 3547f;
                        float damageWithModifiers = baseDamage * player.GetDamage(DamageClass.Ranged).Multiplicative + player.GetDamage(DamageClass.Ranged).Flat;
                        int finalDamage = (int)damageWithModifiers;

                        // 对爆炸范围内的敌人造成伤害（只对敌人，不对玩家）
                        foreach (NPC npc in Main.npc)
                        {
                            if (npc.active && npc.CanBeChasedBy() && !npc.friendly)
                            {
                                float distance = Vector2.Distance(target.Center, npc.Center);
                                if (distance < explosionRadius)
                                {
                                    NPC.HitInfo hitInfo = new NPC.HitInfo();
                                    hitInfo.Damage = finalDamage; // 带有远程伤害加成的伤害
                                    hitInfo.Knockback = 5f;
                                    hitInfo.DamageType = DamageClass.Ranged; // 设置为远程伤害类型
                                    npc.StrikeNPC(hitInfo);
                                }
                            }
                        }

                        // 生成爆炸粒子效果
                        for (int i = 0; i < 150; i++) // 增加粒子数量，与爆炸范围匹配
                        {
                            Vector2 offset = new Vector2(
                                Main.rand.Next(-explosionRadius, explosionRadius + 1),
                                Main.rand.Next(-explosionRadius, explosionRadius + 1)
                            );
                            Vector2 explosionPosition = target.Center + offset;

                            // 生成爆炸粒子
                            int dust = Dust.NewDust(explosionPosition, 10, 10, DustID.Smoke, 0f, 0f, 100, default, 2f);
                            Main.dust[dust].noGravity = true;
                        }
                    }
                }
            }
        }
    }
}
