using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Nothosaur饰品 - 幻龙饰品
    /// </summary>
    internal class MorePlugins_Nothosaur : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Nothosaur";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Lime; // 绿色稀有度
            Item.value = Item.sellPrice(gold: 10); // 售价10金币
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 提升2最大仆从数
            player.maxMinions += 2;

            // 获得水上行走
            player.waterWalk = true;

            // 标记饰品已装备
            player.GetModPlayer<NothosaurPlayer>().nothosaurEquipped = true;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion

        #region 工具提示
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // 添加自定义提示文本
        }
        #endregion
    }

    /// <summary>
    /// Nothosaur饰品的玩家类
    /// </summary>
    public class NothosaurPlayer : ModPlayer
    {
        public bool nothosaurEquipped; // 饰品是否装备
        private int bubbleTimer;

        public override void ResetEffects()
        {
            nothosaurEquipped = false;
        }

        public override void PostUpdate()
        {
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
    }
}