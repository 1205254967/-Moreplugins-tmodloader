using Microsoft.Xna.Framework;
using Moreplugins.Content.Players;
using Moreplugins.Content.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Content.Items.Accessories
{
    internal class DetonatorPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 15);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 标记饰品已装备
            player.GetModPlayer<DetonatorPluginsPlayer>().DetonatorPluginsEquipped = true;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
    }
}

public class DetonatorPluginsPlayer : ModPlayer
{
    public bool DetonatorPluginsEquipped;

    public override void ResetEffects()
    {
        DetonatorPluginsEquipped = false;
    }

    public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
    {
        Projectile.NewProjectile(
                Player.GetSource_Death(),
                Player.Center,
                Vector2.Zero,
                ModContent.ProjectileType<DetonatorPluginsProjectile>(),
                666,                                // 伤害值（可调整）
                5f,                                 // 击退力（可调整）
                Player.whoAmI                       // 发射者玩家ID
            );
        return true;
    }
}