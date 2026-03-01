using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Giantfist饰品 - 巨人之拳
    /// </summary>
    internal class Giantfist : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Giantfist";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(gold: 50);
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GiantfistPlayer>().giantfistEquipped = true;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion
    }

    /// <summary>
    /// Giantfist饰品的玩家类
    /// </summary>
    public class GiantfistPlayer : ModPlayer
    {
        public bool giantfistEquipped;
        private int punchTimer;

        public override void ResetEffects()
        {
            giantfistEquipped = false;
        }

        public override void PostUpdate()
        {
            if (giantfistEquipped)
            {
                punchTimer++;
                if (punchTimer >= 3600) // 1分钟 = 3600帧
                {
                    PunchEnemy();
                    punchTimer = 0;
                }
            }
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
    }
}