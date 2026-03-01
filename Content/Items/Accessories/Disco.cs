using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Content.Items.Accessories;
using Moreplugins.Content.Players;
namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Disco饰品 - 迪斯科棱晶
    /// </summary>
    internal class Disco : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Disco";

        #region 基础属性配置

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<DiscoPlayer>().discoEquipped = true;
            player.maxTurrets += 1; // 增加一哨兵栏位
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion
    }

    /// <summary>
    /// Disco饰品的玩家类
    /// </summary>
    public class DiscoPlayer : ModPlayer
    {
        public bool discoEquipped;
        private int crystalProjectileIndex = -1;

        public override void ResetEffects()
        {
            discoEquipped = false;
        }

        public override void PostUpdate()
        {
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
    }
}