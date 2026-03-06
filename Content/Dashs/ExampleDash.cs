using Moreplugins.Content.Players.DashPlayer;
using Terraria;
using Terraria.ModLoader;

namespace Moreplugins.Content.Dashs
{
    public class ExampleDash : MPPlayerDash
    {
        public override int ImmuneTime(Player player) => 0;
        public override int DashTime(Player player) => 12;
        public override int DashDelay(Player player) => 30;
        public override DashDamageInfo DashDamageInfo(Player player) => new DashDamageInfo(50, 3, DamageClass.Default);
        public override float DashSpeed(Player player) => 12f;
        public override float DashEndSpeedMult(Player player) => 0.25f;
        public override void SetStaticDefaults()
        {
            // 这个为true时会让模版计算将不再生效，搭配ModifyDashSpeed使用
            UseCustomDashSpeed = true;
        }
        public override void OnDashStart(Player player)
        {
            base.OnDashStart(player);
        }
        public override void OnDashEnd(Player player)
        {
            base.OnDashEnd(player);
        }
        public override void OnHitNPC(Player player, NPC target, int DamageDone)
        {
            base.OnHitNPC(player, target, DamageDone);
        }
        public override void ModifyDashDamage(Player player, ref DashDamageInfo dashDamageInfo)
        {
            base.ModifyDashDamage(player, ref dashDamageInfo);
        }
        /// <summary>
        /// 这一段代码只在UseCustomDashSpeed为True时调用，完全接管速度计算，模版计算将不再生效
        /// </summary>
        public override void ModifyDashSpeed(Player player)
        {
            base.ModifyDashSpeed(player);
        }
    }
}
