using Microsoft.Xna.Framework;
using Moreplugins.Content.Projectiles;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Moreplugins.Content.Players
{
    public partial class PluginPlayer : ModPlayer
    {
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            // 雷管插件逻辑
            if (detonatorPluginsEquipped)
            {
                Projectile.NewProjectile(Player.GetSource_Death(), Player.Center, Vector2.Zero, ProjectileType<DetonatorPluginsProjectile>(), 666, 5f, Player.whoAmI);
                return true;
            }

            // 姜饼人插件逻辑
            if (gingerbreadmanPluginsEquipped && !hasUsedEffect)
            {
                Player.Heal(Player.statLifeMax2);
                hasUsedEffect = true;
                return false;
            }
            if (hasUsedEffect)
            {
                hasUsedEffect = false;
                dieTimer = 0;
                return true;
            }
            return true;
        }
        public override void Initialize()
        {
            bleedingNPCs = new Dictionary<int, int>();
        }
    }
}

