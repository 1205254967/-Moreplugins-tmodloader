using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Shaman饰品 - 萨满
    /// </summary>
    internal class Shaman : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Shaman";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(gold: 30);
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ShamanPlayer>().shamanEquipped = true;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion
    }

    /// <summary>
    /// Shaman饰品的玩家类
    /// </summary>
    public class ShamanPlayer : ModPlayer
    {
        public bool shamanEquipped;
        private int attackTimer;

        public override void ResetEffects()
        {
            shamanEquipped = false;
        }

        public override void PostUpdate()
        {
            if (shamanEquipped)
            {
                attackTimer++;
                if (attackTimer >= 60) // 1秒 = 60帧
                {
                    AttackEnemiesWithTentacles();
                    attackTimer = 0;
                }
            }
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
                            496, // 暗影焰娃娃的射弹ID
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
                        npc.AddBuff(153, 300); // 暗影焰增益ID，5秒 = 300帧
                    }
                }
            }
        }
    }
}