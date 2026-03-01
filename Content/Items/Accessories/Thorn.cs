using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Thorn饰品 - 荆棘饰品
    /// </summary>
    internal class MorePlugins_Thorn : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Thorn";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Green; // 绿色稀有度
            Item.value = Item.sellPrice(gold: 2); // 售价2金币
        }
        #endregion

        #region 合成配方
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofLight, 5)              // 5个光明之魂
                .AddIngredient(ItemID.SoulofNight, 5)              // 5个暗影之魂
                .AddIngredient(ItemID.Cactus, 20)                  // 20个仙人掌
                .AddTile(TileID.WorkBenches)                       // 工作台合成
                .Register();
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 标记饰品已装备
            player.GetModPlayer<ThornPlayer>().thornEquipped = true;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion


        #region 工具提示
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // 添加自定义提示文本
        }
        #endregion
    }

    /// <summary>
    /// Thorn饰品的玩家类
    /// </summary>
    public class ThornPlayer : ModPlayer
    {
        public bool thornEquipped; // 饰品是否装备

        public override void ResetEffects()
        {
            thornEquipped = false;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (thornEquipped)
            {
                // 敌人攻击时会受到3倍其造成的伤害
                NPC.HitInfo hitInfo = new NPC.HitInfo();
                hitInfo.Damage = hurtInfo.Damage * 3; // 3倍伤害
                npc.StrikeNPC(hitInfo);
            }
        }

        public override void OnHitByProjectile(Projectile projectile, Player.HurtInfo hurtInfo)
        {
            if (thornEquipped && projectile.hostile)
            {
                // 敌人攻击时会受到3倍其造成的伤害
                if (projectile.owner < 255) // 确保是敌怪发射的弹幕
                {
                    NPC npc = Main.npc[projectile.owner];
                    if (npc.active)
                    {
                        NPC.HitInfo hitInfo = new NPC.HitInfo();
                        hitInfo.Damage = hurtInfo.Damage * 3; // 3倍伤害
                        npc.StrikeNPC(hitInfo);
                    }
                }
            }
        }
    }
}