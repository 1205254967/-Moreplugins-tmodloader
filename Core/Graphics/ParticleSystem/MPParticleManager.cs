using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moreplugins.Content.Particles;
using Moreplugins.Core.SystemLoader;
using Moreplugins.Core.Utilities;
using ReLogic.Threading;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Moreplugins.Core.Graphics.ParticleSystem
{
    public class MPParticleManager : ModSystem
    {
        public static List<MPParticle> ParticlesCollection = [];

        public static List<MPParticle> ActiveParticlesAlpha = [];
        public static List<MPParticle> ActiveParticlesNonPremultiplied = [];
        public static List<MPParticle> ActiveParticlesAdditive = [];
        // 先绘制先更新的粒子
        public static List<MPParticle> PriorityActiveParticlesAlpha = [];
        public static List<MPParticle> PriorityActiveParticlesNonPremultiplied = [];
        public static List<MPParticle> PriorityActiveParticlesAdditive = [];
        public static int TotalDustCount = 0;
        public override void Load()
        {
            On_Main.DrawDust += On_Main_DrawDust_DrawParticles;
        }
        public override void Unload()
        {
            On_Main.DrawDust -= On_Main_DrawDust_DrawParticles;
        }
        public override void OnWorldUnload()
        {
            ParticlesCollection.Clear();
        }
        public override void PostUpdateDusts()
        {
            UpdateParticleList(ActiveParticlesAlpha);
            UpdateParticleList(ActiveParticlesNonPremultiplied);
            UpdateParticleList(ActiveParticlesAdditive);
            UpdateParticleList(PriorityActiveParticlesAlpha);
            UpdateParticleList(PriorityActiveParticlesNonPremultiplied);
            UpdateParticleList(PriorityActiveParticlesAdditive);
            TotalDustCount = ActiveParticlesAlpha.Count + ActiveParticlesNonPremultiplied.Count + ActiveParticlesAdditive.Count + PriorityActiveParticlesAlpha.Count + PriorityActiveParticlesNonPremultiplied.Count + PriorityActiveParticlesAdditive.Count;
        }
        public static void UpdateParticleList(List<MPParticle> list)
        {
            int count = list.Count;
            if (count == 0)
                return;
            FastParallel.For(0, count, (j, k, callback) =>
            {
                for (int i = j; i < k; i++)
                {
                    MPParticle particle = list[i];
                    particle.Update();
                    particle.Position += particle.Velocity;
                    particle.Time++;
                }
            });
            list.RemoveAll(particle =>
            {
                if (particle.Time >= particle.Lifetime)
                {
                    particle.OnKill();
                    return true;
                }
                return false;
            });
        }
        public static void On_Main_DrawDust_DrawParticles(On_Main.orig_DrawDust orig, Main self)
        {
            // 调用源
            orig(self);
            #region 渲染粒子
            #region 渲染优先粒子
            DrawParticles(PriorityActiveParticlesAlpha, BlendState.AlphaBlend);
            DrawParticles(PriorityActiveParticlesAdditive, BlendState.Additive);
            DrawParticles(PriorityActiveParticlesNonPremultiplied, BlendState.NonPremultiplied);
            #endregion
            #region 渲染常规粒子
            DrawParticles(ActiveParticlesAlpha, BlendState.AlphaBlend);
            DrawParticles(ActiveParticlesAdditive, BlendState.Additive);
            DrawParticles(ActiveParticlesNonPremultiplied, BlendState.NonPremultiplied);
            #endregion
            #endregion
        }
        public static void DrawParticles(List<MPParticle> list, BlendState bl)
        {
            if (list.Count != 0)
            {
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, bl, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].UseScreenCut && MPUtils.OutOffScreen(list[i].Position, list[i].ScreenCut))
                        continue;
                    list[i].Draw(Main.spriteBatch);
                }
                Main.spriteBatch.End();
            }
        }
        // 案例生成如何一个粒子
        public static void SpawnButterFly(int Owner)
        {
            // 多人同步，这样可以让生成的粒子精准位于指定玩家的中心，如果调用LocalPlayer
            // 那位于B客户端时，A客户端生成的粒子就会位于B客户端玩家的中心，通过指定的玩家ID来获取中心位置就不会有这个问题了
            Vector2 spawnpos = Main.player[Owner].Center;
            // 速度，Vector2.UnitY为(0,1)，乘以负号就是(0,-1)，再乘以一个随机数就是向上飞的速度，随机数可以自己调整
            Color color = Color.Lerp(Color.White, Color.Gray, Main.rand.NextFloat());
            // 生命周期，随机数可以自己调整
            int timeLeft = Main.rand.Next(60, 120);
            // 湍流的速度
            float Speed = Main.rand.NextFloat(0.5f, 1.5f);
            // 生成粒子实例
            Butterfly butterfly = new (spawnpos, color, timeLeft, 1f, 1f, Speed);
            MPSystemLoader.SpawnParticle(butterfly);
            // 想要指定混合模式就这样写
            MPSystemLoader.SpawnParticle(butterfly, BlendState.AlphaBlend);
            // 绘制到第一层图层就这样指定
            MPSystemLoader.SpawnParticle(butterfly, BlendState.AlphaBlend, true);
            // 或者这样
            MPSystemLoader.SpawnParticle(butterfly, true);
        }
        // 如上
        public static void SpawnCircleParticle(int Owner)
        {
            Vector2 spawnpos = Main.player[Owner].Center;
            Vector2 vel = Main.rand.NextVector2CircularEdge(10, 10);
            Color color = Color.Lerp(Color.White, Color.Gray, Main.rand.NextFloat());
            int timeLeft = Main.rand.Next(60, 120);
            float rot = Main.rand.NextFloat(MathHelper.TwoPi);
            float opacity = 1f;
            float Scale = Main.rand.NextFloat(0.5f, 1.5f);
            Fire fire = new(spawnpos, vel, color, timeLeft, rot, opacity, Scale);
            MPSystemLoader.SpawnParticle(fire, BlendState.NonPremultiplied);
        }
    }
}
