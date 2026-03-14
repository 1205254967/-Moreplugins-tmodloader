using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Moreplugins.Core.Graphics.ParticleSystem;
using Moreplugins.Assets.Textures;

namespace Moreplugins.Content.Particles
{
    public class ShinyOrbParticle : MPParticle
    {
        public override BlendState UseBlendStateID => BlendState.Additive;
        public bool AffectedByGravity = false;
        public bool GlowCenter = true;
        public float FadeOut;
        public Color InitColor;
        public float GlowCenterScale = 0.5f;
        private bool NoLighting;
        public ShinyOrbParticle(Vector2 position, Vector2 velocity, Color color, int lifeTime, float scale, bool noLighting)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = InitColor = color;
            Lifetime = lifeTime;
            Scale = scale;
            FadeOut = 1f;
            NoLighting = noLighting;
        }
        public ShinyOrbParticle(Vector2 position, Vector2 velocity, Color color, int lifeTime, float scale, int? blendState = null, bool affactedByGravity = false, bool glowCenter = true, float glowCenterScale = 0.5f, bool noLighting = false)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = InitColor = color;
            Lifetime = lifeTime;
            Scale = scale;
            AffectedByGravity = affactedByGravity;
            GlowCenter = glowCenter;
            GlowCenterScale = glowCenterScale;
            FadeOut = 1f;
            NoLighting = noLighting;
        }

        public override void Update()
        {
            FadeOut -= 0.05f;
            Scale *= 0.97f;
            DrawColor = Color.Lerp(InitColor, InitColor * 0.2f, (float)Math.Pow(LifetimeRatio, 30));
            if (!NoLighting)
                Lighting.AddLight(Position, new Vector3(DrawColor.R / 255f, DrawColor.G /255f, DrawColor.B/255f));

            Velocity *= 0.95f;
            if(Velocity.Length() < 12f && AffectedByGravity)
            {
                Velocity.X *= 0.94f;
                Velocity.Y += 0.25f;
            }
            Rotation = Velocity.ToRotation() + PiOver2;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 scale = new Vector2(1f, 1f) * Scale;
            Texture2D texture = MPTextureRegister.ShinyOrb.Value;
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor, Rotation, texture.Size() * 0.5f, scale, 0, 0f);
            if (GlowCenter)
                spriteBatch.Draw(texture, Position - Main.screenPosition, null, Color.White, Rotation, texture.Size() * 0.5f, scale * GlowCenterScale, 0, 0f);
        }
    }
}
