using Microsoft.Xna.Framework;
using Moreplugins.Content.Projectiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Content.Players;
public class PluginsPlayer : ModPlayer{
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
    private int crystalProjectileIndex = -1;
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
    #endregion

    public override void ResetEffects()
    {
        soundAcc = false;
        budEquipped = false;
        detonatorPluginsEquipped = false;
        discoEquipped = false;
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
    }
    public override void PostUpdateMiscEffects(){
        if(soundAcc != soundAccOld){
            SoundEngine.PlaySound(DefaultSound);
        }
        soundAccOld = soundAcc;
    }
    public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
    {
        // 雷管插件逻辑
        if (detonatorPluginsEquipped)
        {
            Projectile.NewProjectile(
                    Player.GetSource_Death(),
                    Player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<DetonatorPluginsProjectile>(),
                    666,
                    5f,
                    Player.whoAmI
                );
        }
        return true;
    }
    public override void PostUpdate()
    {
        // 世纪之花插件逻辑
        if (budEquipped)
        {
            timer++;
            if (timer >= 600) // 10秒 = 600帧
            {
                // 恢复40点最大生命值
                int healAmount = 80;
                Player.Heal(healAmount);

                // 产生绿色粒子
                for (int i = 0; i < 20; i++)
                {
                    Vector2 position = Player.Center + new Vector2(Main.rand.Next(-30, 31), Main.rand.Next(-30, 31));
                    Vector2 velocity = new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
                    int dust = Dust.NewDust(position, 10, 10, DustID.JungleSpore, velocity.X, velocity.Y, 100, default, 1.5f);
                    Main.dust[dust].noGravity = true;
                }

                // 产生两个孢子囊弹幕
                SpawnSporePods();

                timer = 0;
            }
        }

        //迪斯科插件逻辑
        if (discoEquipped)
        {
            // 检查七彩水晶是否存在
            if (crystalProjectileIndex == -1 || !Main.projectile[crystalProjectileIndex].active)
            {
                SpawnCrystal();
            }
            else
            {
                // 让水晶跟随玩家移动
                Projectile crystal = Main.projectile[crystalProjectileIndex];
                Vector2 targetPosition = Player.Center + new Vector2(0, -100); // 在玩家头顶
                crystal.position = Vector2.Lerp(crystal.position, targetPosition, 0.1f);
                crystal.netUpdate = true;
            }
        }
        else
        {
            // 如果饰品未装备，移除水晶
            if (crystalProjectileIndex != -1 && Main.projectile[crystalProjectileIndex].active)
            {
                Main.projectile[crystalProjectileIndex].Kill();
                crystalProjectileIndex = -1;
            }
        }

        // 石巨人之拳插件逻辑
        if (giantfistEquipped) 
        {
            punchTimer++;
            if (punchTimer >= 3600) // 1分钟 = 3600帧
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
    public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
    {
        if (thornEquipped)
        {
            // 敌人攻击时会受到3倍其造成的伤害
            NPC.HitInfo hitInfo = new NPC.HitInfo();
            hitInfo.Damage = hurtInfo.Damage * 3; // 3倍伤害
            npc.StrikeNPC(hitInfo);
        }
    }
    public override void OnHitByProjectile(Projectile projectile, Player.HurtInfo hurtInfo)
    {
        if (thornEquipped && projectile.hostile)
        {
            // 敌人攻击时会受到3倍其造成的伤害
            if (projectile.owner < 255) // 确保是敌怪发射的弹幕
            {
                NPC npc = Main.npc[projectile.owner];
                if (npc.active)
                {
                    NPC.HitInfo hitInfo = new NPC.HitInfo();
                    hitInfo.Damage = hurtInfo.Damage * 3; // 3倍伤害
                    npc.StrikeNPC(hitInfo);
                }
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
    public override void Initialize()
    {
        bleedingNPCs = new Dictionary<int, int>();
    }
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
        Projectile crystal = Projectile.NewProjectileDirect(
            Player.GetSource_Accessory(Player.HeldItem),
            Player.Center + new Vector2(0, -100),
            Vector2.Zero,
            ProjectileID.RainbowCrystal, // 原版彩虹水晶哨兵的ID
            1, // 基础伤害，实际伤害由射弹决定
            0f,
            Player.whoAmI
        );

        // 设置为非召唤物，不占用哨兵栏位（参考双子魔眼的实现）
        crystal.minion = false;

        // 记录水晶的索引
        crystalProjectileIndex = crystal.whoAmI;
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

