using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Moreplugins.Core.Graphics.ParticleSystem;
using Moreplugins.Assets.Textures;

namespace Moreplugins.Content.Particles
{
    public class StarShape : MPParticle
    {
        public bool NoGravity = true;
        public Color SparkColor;
        public bool DrawGlow = true;
        public float GlowScale = 0.45f;
        public bool HasRotation;
        public override BlendState UseBlendStateID => BlendState.Additive;
        public StarShape(Vector2 position, Vector2 velocity, Color drawColor, float scale, int lifeTime)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = drawColor;
            Scale = scale;
            Lifetime = lifeTime;
        }
        public StarShape(Vector2 position, Vector2 velocity, Color drawColor, float scale, int lifeTime, float rot)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = drawColor;
            Scale = scale;
            Lifetime = lifeTime;
            HasRotation = true;
            Rotation = rot;
        }
        public StarShape(Vector2 position, Vector2 velocity, Color drawColor, float scale, int lifeTime, int? blendStateID = null, bool noGravity = true, bool drawGlow = true, float glowScale = 0.45f)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = drawColor;
            Scale = scale;
            Lifetime = lifeTime;
            NoGravity = noGravity;
            DrawGlow = drawGlow;
            GlowScale = glowScale;
        }
        public override void Update()
        {
            if (!HasRotation)
                Scale *= 0.95f;
            else
                Scale *= 0.99f;
            SparkColor = Color.Lerp(DrawColor, Color.Transparent, (float)Math.Pow(LifetimeRatio, 3D));
            Velocity *= 0.95f;
            if (Velocity.Length() < 12f && !NoGravity)
            {
                Velocity.X *= 0.94f;
                Velocity.Y += 0.25f;
            }
            Rotation = HasRotation ? Rotation : Velocity.ToRotation() + PiOver2;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 scale = new Vector2(0.5f, 1.6f) * Scale;
            Texture2D texture = MPTextureRegister.SharpTear;
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, SparkColor, Rotation, texture.Size() * 0.5f, scale, 0, 0f);
            if (DrawGlow)
                spriteBatch.Draw(texture, Position - Main.screenPosition, null, Color.White, Rotation, texture.Size() * 0.5f, scale * new Vector2(GlowScale, 1f), 0, 0f);
        }
    }
}
