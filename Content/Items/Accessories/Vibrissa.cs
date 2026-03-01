using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Vibrissa饰品 - 触须
    /// </summary>
    internal class Vibrissa : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Vibrissa";

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

        #region 合成配方
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BeeWax, 20)          // 20个蜂蜡
                .AddTile(TileID.TinkerersWorkbench)         // 工匠作坊合成
                .Register();
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 标记饰品已装备
            player.GetModPlayer<VibrissaPlayer>().vibrissaEquipped = true;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion
    }

    /// <summary>
    /// Vibrissa饰品的玩家类
    /// </summary>
    public class VibrissaPlayer : ModPlayer
    {
        public bool vibrissaEquipped; // 饰品是否装备
        private int beeTimer;
        private int hornetProjectileId1 = -1; // 存储第一个黄蜂的projectile ID
        private int hornetProjectileId2 = -1; // 存储第二个黄蜂的projectile ID

        public override void ResetEffects()
        {
            vibrissaEquipped = false;
        }

        public override void PostUpdate()
        {
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