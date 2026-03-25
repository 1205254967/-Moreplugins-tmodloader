using Moreplugins.Content.Players.DashPlayer;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Moreplugins.Content.Dashs
{
    public class SwordfishDash : MPPlayerDash
    {
        public override int ImmuneTime(Player player) => 0;
        public override int DashTime(Player player) => 12;
        public override int DashDelay(Player player) => player.wet || Main.raining ? 30 : 45;
        public override DashDamageInfo DashDamageInfo(Player player) => new DashDamageInfo(15, 3, DamageClass.Default);
        public override float DashSpeed(Player player) => player.wet || Main.raining ? 21 : 12f;
        public override float DashEndSpeedMult(Player player) => 0.25f;
        public override void SetStaticDefaults()
        {
            // 这个为true时会让模版计算将不再生效，搭配ModifyDashSpeed使用
            UseCustomDashSpeed = true;
        }
        public override void OnDashStart(Player player)
        {
            base.OnDashStart(player);
            player.Hurt(PlayerDeathReason.ByCustomReason(NetworkText.FromKey(player.name + Language.GetTextValue("Mods.Moreplugins.DeathMessage.SwordfishDash"))), 5, 0, true, true);
        }
        public override void OnDashEnd(Player player)
        {
            base.OnDashEnd(player);
            for (int j = 0; j < 60; j++)
            {
                if (Main.rand.NextBool(4))
                {
                    Dust.NewDustDirect(player.Center, 1, 1, DustID.Poop, 2f, 2f, 100, default, 1f);
                }
                Dust.NewDustDirect(player.Center, 1, 1, DustID.Blood, 2f, 2f, 100, default, 1f);
            }
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
