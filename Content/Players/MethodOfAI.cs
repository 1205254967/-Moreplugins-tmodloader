using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Moreplugins.Content.Players
{
    public partial class PluginPlayer : ModPlayer
    {
        // 这些都是需要重写的方法，请勿新增，求求了，要新建方法可以新建一个Method文件，谁要是在这写我自刎归天——那年锦绣烟成雨
        private void SpawnSporePods()
        {
            // 产生第一个孢子囊弹幕
            Projectile.NewProjectileDirect(
                Player.GetSource_Accessory(Player.HeldItem),
                Player.Center + new Vector2(-30, 0),
                new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)),
                ProjectileID.SporeCloud, // 孢子囊弹幕
                20, // 伤害
                2f, // 击退
                Player.whoAmI
            );

            // 产生第二个孢子囊弹幕
            Projectile.NewProjectileDirect(
                Player.GetSource_Accessory(Player.HeldItem),
                Player.Center + new Vector2(30, 0),
                new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)),
                ProjectileID.SporeCloud, // 孢子囊弹幕
                20, // 伤害
                2f, // 击退
                Player.whoAmI
            );
        }
        private void SpawnCrystal()
        {
            // 生成原版的彩虹水晶哨兵
            Projectile crystal = Projectile.NewProjectileDirect(Player.GetSource_Accessory(Player.HeldItem), Player.Center + new Vector2(0, -100), Vector2.Zero, ProjectileID.RainbowCrystal, 1, 0f, Player.whoAmI);
            crystal.minion = false;
            crystalIndex = crystal.whoAmI;
        }
        private void PunchEnemy()
        {
            // 寻找附近的敌人
            NPC target = FindNearestEnemy();
            if (target != null)
            {
                // 生成石巨人之拳的射弹
                Projectile punch = Projectile.NewProjectileDirect(
                    Player.GetSource_Accessory(Player.HeldItem),
                    Player.Center,
                    Vector2.Normalize(target.Center - Player.Center) * 10f,
                    ProjectileID.GolemFist,
                    20000, // 20000伤害
                    10f,
                    Player.whoAmI
                );

                // 设置射弹属性
                punch.tileCollide = false;
                punch.friendly = true;
                punch.hostile = false;
                punch.owner = Player.whoAmI;
                punch.timeLeft = 600; // 10秒
                punch.netUpdate = true;
            }
        }
        private NPC FindNearestEnemy()
        {
            NPC target = null;
            float minDistance = float.MaxValue;
            float searchRadius = 1000f;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.CanBeChasedBy() && !npc.friendly)
                {
                    float distance = Vector2.Distance(Player.Center, npc.Center);
                    if (distance < searchRadius && distance < minDistance)
                    {
                        minDistance = distance;
                        target = npc;
                    }
                }
            }

            return target;
        }
        private void SpawnBloodThornProjectiles()
        {
            // 寻找附近的敌人（最多3个）
            List<NPC> targets = FindNearestEnemies(3);
            foreach (NPC target in targets)
            {
                // 对每个敌人生成3个血荆棘
                for (int i = 0; i < 3; i++)
                {
                    // 计算敌人周围的随机位置（距离更近）
                    Vector2 offset = new Vector2(
                        Main.rand.Next(-50, 51), // 减小生成范围，距离敌人更近
                        Main.rand.Next(-50, 51)
                    );
                    Vector2 spawnPosition = target.Center + offset;

                    // 生成血荆棘弹幕（使用原版血荆棘的射弹ID）
                    Projectile projectile = Projectile.NewProjectileDirect(
                        Player.GetSource_Accessory(Player.HeldItem),
                        spawnPosition,
                        Vector2.Zero, // 初始速度为0，让血荆棘自己生长
                        ProjectileID.SharpTears, // 血荆棘的射弹ID
                        30,
                        3f,
                        Player.whoAmI
                    );

                    // 设置血荆棘属性
                    projectile.penetrate = -1; // 无限穿透
                    projectile.timeLeft = 60; // 1秒持续时间
                    projectile.tileCollide = false; // 不与物块碰撞
                    projectile.netUpdate = true;
                }
            }
        }
        private List<NPC> FindNearestEnemies(int maxCount)
        {
            List<NPC> targets = new List<NPC>();
            List<(NPC npc, float distance)> enemyDistances = new List<(NPC, float)>();
            float searchRadius = 800f;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.CanBeChasedBy() && !npc.friendly)
                {
                    float distance = Vector2.Distance(Player.Center, npc.Center);
                    if (distance < searchRadius)
                    {
                        enemyDistances.Add((npc, distance));
                    }
                }
            }

            // 按距离排序并取最近的maxCount个敌人
            enemyDistances.Sort((a, b) => a.distance.CompareTo(b.distance));
            for (int i = 0; i < Math.Min(maxCount, enemyDistances.Count); i++)
            {
                targets.Add(enemyDistances[i].npc);
            }

            return targets;
        }
        private void SpawnBubble()
        {
            // 寻找附近的敌人
            NPC target = null;
            float minDistance = float.MaxValue;
            float searchRadius = 100 * 16; // 100格 = 20格 * 5

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.CanBeChasedBy() && !npc.friendly)
                {
                    float distance = Vector2.Distance(Player.Center, npc.Center);
                    if (distance < searchRadius && distance < minDistance)
                    {
                        minDistance = distance;
                        target = npc;
                    }
                }
            }

            if (target != null)
            {
                // 从玩家位置生成泡泡
                Vector2 spawnPosition = Player.Center;
                Vector2 velocity = Vector2.Normalize(target.Center - spawnPosition) * 15f; // 速度提升至原来的三倍

                // 生成原版泡泡弹幕
                Projectile projectile = Projectile.NewProjectileDirect(
                    Projectile.GetSource_NaturalSpawn(),
                    spawnPosition,
                    velocity,
                    ProjectileID.Bubble,
                    180, // 基础伤害
                    0f,
                    Player.whoAmI
                );

                // 增加泡泡的生命周期，确保它能到达远处的敌人
                projectile.timeLeft = 600; // 足够到达远处的敌人
            }
        }
        private void SpawnFirstNuke()
        {
            // 寻找附近的敌人
            NPC target = FindNearestEnemy();
            if (target != null)
            {
                // 计算发射方向（雪人大炮攻击模板）
                Vector2 direction = Vector2.Normalize(target.Center - Player.Center);
                float speed = 10f; // 发射速度

                // 发射第一颗火箭（火箭一型）
                Projectile nuke1 = Projectile.NewProjectileDirect(
                    Player.GetSource_Accessory(Player.HeldItem),
                    Player.Center,
                    direction * speed,
                    ProjectileID.RocketI, // 火箭一型
                    1, // 最小伤害，确保OnHitNPC触发
                    0f, // 无击退
                    Player.whoAmI
                );
                // 设置火箭属性（雪人大炮风格）
                nuke1.tileCollide = false; // 不破坏物块
                nuke1.friendly = true; // 对敌人造成伤害
                nuke1.hostile = false; // 不对玩家造成伤害
                nuke1.owner = Player.whoAmI; // 设置所有者
                nuke1.maxPenetrate = 1; // 只穿透一次，确保爆炸触发
                nuke1.penetrate = 1; // 只穿透一次，确保爆炸触发
                nuke1.usesLocalNPCImmunity = true;
                nuke1.localNPCHitCooldown = -1;
                nuke1.timeLeft = 3600; // 60秒
                nuke1.netUpdate = true;
                // 增加爆炸半径为3倍
                nuke1.scale = 3f;

                // 设置强追踪效果（完全模拟雪人大炮，持续追踪移动的敌怪）
                nuke1.ai[0] = target.whoAmI; // 目标NPC的ID
                nuke1.ai[1] = 1f; // 启用强追踪模式
                                  // 设置标记，用于识别这是核弹饰品发射的导弹
                nuke1.ai[2] = 1f; // 使用ai[2]作为标记
                nuke1.netUpdate = true;
            }
        }
        private int SpawnStarCell()
        {
            // 计算基础伤害，与星辰细胞法杖相同（基础伤害28）
            int damage = (int)(28 * Player.GetDamage(DamageClass.Summon).Additive);

            // 召唤星辰细胞
            Projectile starCell = Projectile.NewProjectileDirect(
                Player.GetSource_Accessory(Player.HeldItem),
                Player.Center,
                Vector2.Zero,
                ProjectileID.StardustCellMinion,
                damage,
                0f,
                Player.whoAmI
            );

            // 设置为非召唤物，不占用召唤栏位
            starCell.minion = false;

            // 返回projectile ID
            return starCell.whoAmI;
        }
        private void AttackEnemiesWithTentacles()
        {
            // 寻找附近的敌人
            float searchRadius = 800f;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.CanBeChasedBy() && !npc.friendly)
                {
                    float distance = Vector2.Distance(Player.Center, npc.Center);
                    if (distance < searchRadius)
                    {
                        // 计算攻击方向
                        Vector2 direction = Vector2.Normalize(npc.Center - Player.Center);
                        float speed = 8f;

                        // 生成暗影焰娃娃的触手攻击
                        Projectile tentacle = Projectile.NewProjectileDirect(
                            Player.GetSource_Accessory(Player.HeldItem),
                            Player.Center,
                            direction * speed,
                            ProjectileID.ShadowFlame, // 暗影焰娃娃的射弹ID
                            25, // 与原版暗影焰娃娃相同的伤害
                            1f,
                            Player.whoAmI
                        );

                        // 设置射弹属性
                        tentacle.tileCollide = false;
                        tentacle.friendly = true;
                        tentacle.hostile = false;
                        tentacle.owner = Player.whoAmI;
                        tentacle.timeLeft = 600; // 10秒
                        tentacle.netUpdate = true;

                        // 对敌人造成暗影焰效果
                        npc.AddBuff(BuffID.ShadowFlame, 300); // 暗影焰增益ID，5秒 = 300帧
                    }
                }
            }
        }
        private void SpawnTwins()
        {
            // 检查已存储的双子魔眼是否仍然存在
            bool spazmaminiExists = spazmaminiProjectileId != -1 && Main.projectile[spazmaminiProjectileId].active;
            bool retaniminiExists = retaniminiProjectileId != -1 && Main.projectile[retaniminiProjectileId].active;

            // 只在缺少时召唤，确保每种最多只有一个
            if (!spazmaminiExists || !retaniminiExists)
            {
                // 计算双子眼的伤害，与原版双魔眼法杖相同
                int damage = (int)(60 * Player.GetDamage(DamageClass.Summon).Additive);

                // 召唤缺失的Spazmamini
                if (!spazmaminiExists)
                {
                    Projectile spazmamini = Projectile.NewProjectileDirect(
                        Player.GetSource_Accessory(Player.HeldItem),
                        Player.Center,
                        Vector2.Zero,
                        ProjectileID.Spazmamini,
                        damage,
                        2f,
                        Player.whoAmI
                    );
                    // 存储projectile ID
                    spazmaminiProjectileId = spazmamini.whoAmI;
                    // 设置为非召唤物，不占用召唤栏位
                    spazmamini.minion = false;
                }

                // 召唤缺失的Retanimini
                if (!retaniminiExists)
                {
                    Projectile retanimini = Projectile.NewProjectileDirect(
                        Player.GetSource_Accessory(Player.HeldItem),
                        Player.Center,
                        Vector2.Zero,
                        ProjectileID.Retanimini,
                        damage,
                        2f,
                        Player.whoAmI
                    );
                    // 存储projectile ID
                    retaniminiProjectileId = retanimini.whoAmI;
                    // 设置为非召唤物，不占用召唤栏位
                    retanimini.minion = false;
                }
            }
        }
        private void SpawnLaser()
        {
            // 寻找附近的敌人
            NPC target = null;
            float minDistance = float.MaxValue;
            float searchRadius = 800f; // 搜索半径

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.CanBeChasedBy() && !npc.friendly)
                {
                    float distance = Vector2.Distance(Player.Center, npc.Center);
                    if (distance < searchRadius && distance < minDistance)
                    {
                        minDistance = distance;
                        target = npc;
                    }
                }
            }

            if (target != null)
            {
                // 随机选择激光类型
                int laserType = Main.rand.Next(4);
                Vector2 direction = Vector2.Normalize(target.Center - Player.Center);
                int damage = 0;
                Projectile projectile = null;

                switch (laserType)
                {
                    case 0: // 红色激光
                        damage = 50;
                        projectile = Projectile.NewProjectileDirect(
                            Player.GetSource_Accessory(Player.HeldItem),
                            Player.Center,
                            direction * 10f,
                            ProjectileID.DeathLaser,
                            damage,
                            1f,
                            Player.whoAmI
                        );
                        break;
                    case 1: // 绿色激光
                        damage = 20;
                        projectile = Projectile.NewProjectileDirect(
                            Player.GetSource_Accessory(Player.HeldItem),
                            Player.Center,
                            direction * 10f,
                            ProjectileID.GreenLaser,
                            damage,
                            1f,
                            Player.whoAmI
                        );
                        break;
                    case 2: // 黄色激光
                        damage = 15;
                        projectile = Projectile.NewProjectileDirect(
                            Player.GetSource_Accessory(Player.HeldItem),
                            Player.Center,
                            direction * 10f,
                            ProjectileID.VortexLaser,
                            damage,
                            1f,
                            Player.whoAmI
                        );
                        break;
                    case 3: // 蓝色激光
                        damage = 30;
                        projectile = Projectile.NewProjectileDirect(
                            Player.GetSource_Accessory(Player.HeldItem),
                            Player.Center,
                            direction * 10f,
                            ProjectileID.NebulaLaser,
                            damage,
                            1f,
                            Player.whoAmI
                        );
                        break;
                }

                if (projectile != null)
                {
                    // 标记为饰品召唤的激光
                    projectile.ai[1] = 99f;
                    // 设置激光只攻击敌人，不伤害玩家
                    projectile.friendly = true;
                    projectile.hostile = false;
                    projectile.tileCollide = false;
                }
            }
        }
        private void SpawnHornets()
        {
            // 检查已存储的黄蜂是否仍然存在
            bool hornet1Exists = hornetProjectileId1 != -1 && Main.projectile[hornetProjectileId1].active;
            bool hornet2Exists = hornetProjectileId2 != -1 && Main.projectile[hornetProjectileId2].active;

            // 计算需要召唤的黄蜂数量
            int hornetsToSpawn = 0;
            if (!hornet1Exists)
            {
                hornetsToSpawn++;
            }
            if (!hornet2Exists)
            {
                hornetsToSpawn++;
            }

            // 只在缺少时召唤，确保最多只有两个
            if (hornetsToSpawn > 0)
            {
                // 计算黄蜂的伤害，与原版黄蜂法杖相同
                int damage = (int)(18 * Player.GetDamage(DamageClass.Summon).Additive);

                // 召唤缺失的黄蜂
                if (!hornet1Exists)
                {
                    Projectile hornet = Projectile.NewProjectileDirect(
                        Player.GetSource_Accessory(Player.HeldItem),
                        Player.Center,
                        Vector2.Zero,
                        ProjectileID.Hornet,
                        damage,
                        2f,
                        Player.whoAmI
                    );
                    // 存储projectile ID
                    hornetProjectileId1 = hornet.whoAmI;
                    // 设置为非召唤物，不占用召唤栏位
                    hornet.minion = false;
                }

                if (!hornet2Exists)
                {
                    Projectile hornet = Projectile.NewProjectileDirect(
                        Player.GetSource_Accessory(Player.HeldItem),
                        Player.Center,
                        Vector2.Zero,
                        ProjectileID.Hornet,
                        damage,
                        2f,
                        Player.whoAmI
                    );
                    // 存储projectile ID
                    hornetProjectileId2 = hornet.whoAmI;
                    // 设置为非召唤物，不占用召唤栏位
                    hornet.minion = false;
                }
            }
        }
    }
}