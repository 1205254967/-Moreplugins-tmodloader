using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System;
using Moreplugins.Content.Players;


namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Bud饰品 - 花苞
    /// </summary>
    internal class Bud : BasicPlugins

    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Bud";

        #region 基础属性配置

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Pink; // 粉色稀有度
            Item.value = Item.sellPrice(gold: 15); // 售价15金币
        }
        #endregion

        #region 合成配方（可选）
        public override void AddRecipes()
        {
            // 可以添加合成配方，或者只作为掉落物品
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 标记饰品已装备
            player.GetModPlayer<BudPlayer>().budEquipped = true;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion
    }

    /// <summary>
    /// Bud饰品的玩家类
    /// </summary>
    public class BudPlayer : ModPlayer
    {
        public bool budEquipped; // 饰品是否装备
        private int timer;

        public override void ResetEffects()
        {
            budEquipped = false;
        }

        public override void PostUpdate()
        {
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
        }

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
    }
}