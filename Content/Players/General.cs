using Microsoft.Xna.Framework;
using Moreplugins.Content.Projectiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Content.Players
{
    public partial class PluginPlayer : ModPlayer
    {
        #region 设置变量
        // 插拔音效用
        public bool soundAcc = false;
        public bool soundAccOld = false;
        public static SoundStyle DefaultSound = new SoundStyle("Moreplugins/Assets/Sounds/Accessories/bobobo");
        // 世纪小花客串用
        public bool budEquipped;
        private int timer;
        // 雷管插件用
        public bool detonatorPluginsEquipped;
        // 迪斯科插件用
        public bool discoEquipped;
        private int crystalIndex = -1;
        // 黄昏插件用
        public bool duskEquipped;
        // 附魔插件用
        public bool enchantedacc;
        // 幽魂插件用
        public bool ghostEquipped;
        // 石巨人之拳插件用
        public bool giantfistEquipped;
        private int punchTimer;
        // 村正插件用
        public bool kaishakuninEquipped; // 饰品是否装备
        public int boostTimer; // 伤害提升计时器
        public bool damageBoostActive; // 伤害提升是否激活
                                       // 熔岩之心插件用
        public bool lavaSeedEquipped;
        // 血腥插件用
        public bool massacreEquipped; // 饰品是否装备
        private Dictionary<int, int> bleedingNPCs; // 存储流血的NPC和计时器
                                                   // 肉球插件用
        public bool meatballEquipped;
        private int thornTimer;
        // 永夜插件用
        public bool nightEquipped; // 饰品是否装备
        private int attackTimer;
        // 幻龙插件用
        public bool nothosaurEquipped; // 饰品是否装备
        private int bubbleTimer;
        // 核弹插件用
        public bool nuclearWarheadEquipped; // 饰品是否装备
        private int nukeTimer;
        // 户外生存装置插件用
        public bool kitEquipped; // 饰品是否装备
        private int laserTimer;
        private int eyeTimer;
        private int spazmaminiProjectileId = -1; // 存储Spazmamini的 projectile ID
        private int retaniminiProjectileId = -1; // 存储Retanimini的 projectile ID
                                                 // 纯净插件用
        public bool pureEquipped; // 饰品是否装备
                                  // 萨满插件用
        public bool shamanEquipped;
        private int samanAttackTimer;
        // 腐化插件用
        public bool shadowyeggplantEquipped;
        // 荆棘插件用
        public bool thornEquipped;
        // 泰拉之心插件用
        public bool terraHeartEquipped; // 饰品是否装备
        public int terraHeartAttackTimer;
        public bool terraHeartAamageBoostActive;
        private bool isBoostedHit; // 标记是否是提升后的伤害
                                   // 团结插件用
        public bool unityEquipped;
        private int[] starCellProjectileIds = new int[3] { -1, -1, -1 };
        // 黄蜂插件用
        public bool vibrissaEquipped; // 饰品是否装备
        private int beeTimer;
        private int hornetProjectileId1 = -1; // 存储第一个黄蜂的projectile ID
        private int hornetProjectileId2 = -1; // 存储第二个黄蜂的projectile ID
        // 姜饼人插件用
        public bool gingerbreadmanPluginsEquipped;
        public bool hasUsedEffect = false;
        public int dieTimer;

        //神圣
        public bool holyPlugin = false;

        public float summonCritChance = 0f;
        public float summonCritDamageMultipler = 0f;
        #endregion

        public override void ResetEffects()
        {
            soundAcc = false;
            budEquipped = false;
            detonatorPluginsEquipped = false;
            discoEquipped = false;
            duskEquipped = false;
            enchantedacc = false;
            ghostEquipped = false;
            giantfistEquipped = false;
            kaishakuninEquipped = false;
            lavaSeedEquipped = false;
            massacreEquipped = false;
            meatballEquipped = false;
            nightEquipped = false;
            nothosaurEquipped = false;
            nuclearWarheadEquipped = false;
            kitEquipped = false;
            pureEquipped = false;
            shamanEquipped = false;
            shadowyeggplantEquipped = false;
            thornEquipped = false;
            unityEquipped = false;
            terraHeartEquipped = false;
            vibrissaEquipped = false;
            gingerbreadmanPluginsEquipped = false;
            summonCritDamageMultipler = 0f;
            summonCritChance = 0f;
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            // 雷管插件逻辑
            if (detonatorPluginsEquipped)
            {
                Projectile.NewProjectile(Player.GetSource_Death(), Player.Center, Vector2.Zero, ProjectileType<DetonatorPluginsProjectile>(), 666, 5f, Player.whoAmI);
                return true;
            }

            // 姜饼人插件逻辑
            if (gingerbreadmanPluginsEquipped && !hasUsedEffect)
            {
                Player.Heal(Player.statLifeMax2);
                hasUsedEffect = true;
                return false;
            }
            if (hasUsedEffect)
            {
                hasUsedEffect = false;
                dieTimer = 0;
                return true;
            }
            return true;
        }
                public override void Initialize()
        {
            bleedingNPCs = new Dictionary<int, int>();
        }
        //===================================下列为方法========================================
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

