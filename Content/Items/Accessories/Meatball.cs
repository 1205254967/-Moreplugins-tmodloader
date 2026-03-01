using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Meatball饰品 - 肉球
    /// </summary>
    internal class Meatball : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Meatball";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(gold: 30);
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MeatballPlayer>().meatballEquipped = true;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion
    }

    /// <summary>
    /// Meatball饰品的玩家类
    /// </summary>
    public class MeatballPlayer : ModPlayer
    {
        public bool meatballEquipped;
        private int thornTimer;

        public override void ResetEffects()
        {
            meatballEquipped = false;
        }

        public override void PostUpdate()
        {
            if (meatballEquipped)
            {
                thornTimer++;
                if (thornTimer >= 240) // 2秒 = 120帧
                {
                    SpawnBloodThornProjectiles();
                    thornTimer = 0;
                }
            }
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
                        756, // 血荆棘的射弹ID
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
    }
}
