using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace Moreplugins
{
    using Content.Items.Accessories;

    /// <summary>
    /// 处理迪斯科棱晶的水晶射弹伤害
    /// </summary>
    public class DiscoProjectile : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            // 检查是否是彩虹水晶发射的射弹
            if (projectile.type == ProjectileID.RainbowCrystalExplosion)
            {
                // 检查射弹的所有者是否装备了迪斯科棱晶
                Player player = Main.player[projectile.owner];
                if (player.active)
                {
                    DiscoPlayer modPlayer = player.GetModPlayer<DiscoPlayer>();
                    if (modPlayer.discoEquipped)
                    {
                        // 设置射弹伤害为500
                        projectile.damage = 240;
                        projectile.netUpdate = true;
                    }
                }
            }
        }
    }
}