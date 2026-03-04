using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Content.Items.Accessories
{
    internal class PumpkinCandlePlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);

            // 检查玩家是否正在使用无头骑士剑
            if (player.HeldItem.type == ItemID.TheHorsemansBlade && player.itemAnimation > 0)
            {
                // 当玩家挥舞无头骑士剑时，生成烈焰杰克南瓜射弹
                // 与原版无头骑士剑相同的生成时机
                if (player.itemAnimation == player.itemAnimationMax - 1)
                {
                    SpawnFlamingJack(player);
                }
            }
        }

        private void SpawnFlamingJack(Player player)
        {
            // 获取玩家当前使用的无头骑士剑
            Item sword = player.HeldItem;

            // 计算伤害和暴击率，与剑本身相同
            int damage = sword.damage;
            float knockback = sword.knockBack;
            bool crit = Main.rand.NextFloat() < sword.crit / 100f;

            // 寻找最近的敌人
            NPC target = null;
            float closestDistance = float.MaxValue;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && !npc.dontTakeDamage && npc.lifeMax > 5)
                {
                    float distance = Vector2.Distance(player.Center, npc.Center);
                    if (distance < closestDistance && distance < 1600) // 扩大搜索范围
                    {
                        closestDistance = distance;
                        target = npc;
                    }
                }
            }

            // 如果找到目标，生成两个烈焰杰克南瓜射弹（数量*2）
            if (target != null)
            {
                int projectileType = ProjectileID.FlamingJack;

                // 生成两个射弹，每个在不同的随机方向
                for (int i = 0; i < 1; i++)
                {
                    // 在屏幕四周随机位置生成，与原版无头骑士剑相同
                    Vector2 spawnPosition;
                    int side = Main.rand.Next(4); // 0: 顶部, 1: 右侧, 2: 底部, 3: 左侧

                    // 随机生成位置
                    switch (side)
                    {
                        case 0: // 顶部
                            spawnPosition = new Vector2(Main.rand.Next(Main.screenWidth), -50);
                            break;
                        case 1: // 右侧
                            spawnPosition = new Vector2(Main.screenWidth + 50, Main.rand.Next(Main.screenHeight));
                            break;
                        case 2: // 底部
                            spawnPosition = new Vector2(Main.rand.Next(Main.screenWidth), Main.screenHeight + 50);
                            break;
                        default: // 左侧
                            spawnPosition = new Vector2(-50, Main.rand.Next(Main.screenHeight));
                            break;
                    }

                    // 转换为世界坐标
                    spawnPosition += Main.screenPosition;

                    // 计算朝向目标的速度
                    Vector2 velocity = Vector2.Normalize(target.Center - spawnPosition) * 8f;

                    Projectile.NewProjectile(
                        player.GetSource_FromThis(),
                        spawnPosition,
                        velocity,
                        projectileType,
                        damage, // 与剑相同的伤害
                        knockback, // 与剑相同的击退
                        player.whoAmI,
                        target.whoAmI, // 目标ID
                        crit ? 1f : 0f // 暴击率
                    );
                }
            }
        }
    }
}
