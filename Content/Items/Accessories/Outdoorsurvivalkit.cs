using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Outdoorsurvivalkit饰品 - 户外生存装置
    /// </summary>
    internal class Outdoorsurvivalkit : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Outdoorsurvivalkit";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Pink; // 粉色稀有度
            Item.value = Item.sellPrice(gold: 10); // 售价10金币
        }
        #endregion

        #region 合成配方
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofFright, 1)      // 1个恐惧之魂
                .AddIngredient(ItemID.SoulofSight, 1)       // 1个视域之魂
                .AddIngredient(ItemID.SoulofMight, 1)       // 1个力量之魂
                .AddIngredient(ItemID.SkeletronPrimePetItem, 1) // 机械骷髅王的大师模式奖励宠物
                .AddIngredient(ItemID.TwinsPetItem, 1)      // 双子魔眼的大师模式奖励宠物
                .AddIngredient(ItemID.DestroyerPetItem, 1)   // 毁灭者的大师模式奖励宠物
                .AddIngredient(ItemID.Wire, 15)             // 15根电线
                .AddIngredient(ItemID.IronBar, 20)          // 20个铁锭
                .AddTile(TileID.MythrilAnvil)               // 秘银砧/山铜砧合成
                .Register();
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 标记饰品已装备
            player.GetModPlayer<OutdoorsurvivalkitPlayer>().kitEquipped = true;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion
    }

    /// <summary>
    /// Outdoorsurvivalkit饰品的玩家类
    /// </summary>
    public class OutdoorsurvivalkitPlayer : ModPlayer
    {
        public bool kitEquipped; // 饰品是否装备
        private int laserTimer;
        private int eyeTimer;
        private int spazmaminiProjectileId = -1; // 存储Spazmamini的 projectile ID
        private int retaniminiProjectileId = -1; // 存储Retanimini的 projectile ID

        public override void ResetEffects()
        {
            kitEquipped = false;
        }

        public override void PostUpdate()
        {
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
    }
}