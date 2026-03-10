using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Moreplugins.Content.Pets.Steel
{
    public class SteelPlusinsPetProjectile : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ZephyrFish);
            AIType = ProjectileID.ZephyrFish;
            Projectile.scale = 3f;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            player.zephyrfish = false;
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            
            Projectile.localAI[0] += 0.1f;
            Projectile.rotation = Projectile.localAI[0];
            Projectile.spriteDirection = 1;
            if (!player.dead && player.HasBuff(ModContent.BuffType<SteelPlusinsPetBuff>()))
            {
                Projectile.timeLeft = 2;
            }
        }

        public override bool PreDraw(ref Color lightColor)		
        {
            Texture2D projTex = ModContent.Request<Texture2D>(Texture).Value;
            Main.spriteBatch.Draw(projTex, Projectile.Center - Main.screenPosition,
            null, Color.White, Projectile.rotation, projTex.Size() / 2, Projectile.scale, 0, 0f);
            return false;
        }
    }
}