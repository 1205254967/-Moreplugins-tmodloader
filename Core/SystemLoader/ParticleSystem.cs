using Microsoft.Xna.Framework.Graphics;
using Moreplugins.Core.Graphics.ParticleSystem;
using Terraria;

namespace Moreplugins.Core.SystemLoader
{
    public static class MPSystemLoader
    {
        public static int ParticleType<T>() where T : MPParticle => GetInstance<T>()?.Type ?? 0;
        public static void SpawnParticle(MPParticle mPParticle, bool priority = false)
        {
            
            if (MPParticleManager.TotalDustCount > 10000)
                return;
            BlendState blendstateid = mPParticle.UseBlendStateID;
            if (priority)
            {
                if (blendstateid == BlendState.Additive)
                    MPParticleManager.PriorityActiveParticlesAdditive.Add(mPParticle);
                else if (blendstateid == BlendState.NonPremultiplied)
                    MPParticleManager.PriorityActiveParticlesNonPremultiplied.Add(mPParticle);
                else
                    MPParticleManager.PriorityActiveParticlesAlpha.Add(mPParticle);
            }
            else
            {
                if (blendstateid == BlendState.Additive)
                    MPParticleManager.ActiveParticlesAdditive.Add(mPParticle);
                else if (blendstateid == BlendState.NonPremultiplied)
                    MPParticleManager.ActiveParticlesNonPremultiplied.Add(mPParticle);
                else
                    MPParticleManager.ActiveParticlesAlpha.Add(mPParticle);
            }
        }
        public static void SpawnParticle(MPParticle mPParticle, BlendState state, bool priority = false)
        {
            if (MPParticleManager.TotalDustCount > 10000)
                return;
            BlendState blendstateid = state;
            if (priority)
            {
                if (blendstateid == BlendState.Additive)
                    MPParticleManager.PriorityActiveParticlesAdditive.Add(mPParticle);
                else if (blendstateid == BlendState.NonPremultiplied)
                    MPParticleManager.PriorityActiveParticlesNonPremultiplied.Add(mPParticle);
                else
                    MPParticleManager.PriorityActiveParticlesAlpha.Add(mPParticle);
            }
            else
            {
                if (blendstateid == BlendState.Additive)
                    MPParticleManager.ActiveParticlesAdditive.Add(mPParticle);
                else if (blendstateid == BlendState.NonPremultiplied)
                    MPParticleManager.ActiveParticlesNonPremultiplied.Add(mPParticle);
                else
                    MPParticleManager.ActiveParticlesAlpha.Add(mPParticle);
            }
        }
    }
}
