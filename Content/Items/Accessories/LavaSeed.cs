using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// LavaSeed饰品
    /// </summary>
    internal class MorePlugins_LavaSeed : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/LavaSeed";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Orange; // 橙色稀有度
            Item.value = Item.sellPrice(gold: 3); // 售价3金币
        }
        #endregion

        #region 合成配方
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 10)        // 10个狱石锭
                .AddIngredient(ItemID.LavaBucket, 1)          // 1个岩浆桶
                .AddIngredient(ItemID.Fireblossom, 5)         // 5个火焰花
                .AddTile(TileID.Anvils)                      // 铁砧/铅砧合成
                .Register();
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 提升5点防御
            player.statDefense += 5;

            // 提升5%的伤害减免
            player.endurance += 0.05f;

            // 标记饰品已装备
            player.GetModPlayer<LavaSeedPlayer>().lavaSeedEquipped = true;
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
    /// LavaSeed饰品的玩家类
    /// </summary>
    public class LavaSeedPlayer : ModPlayer
    {
        public bool lavaSeedEquipped; // 饰品是否装备

        public override void ResetEffects()
        {
            lavaSeedEquipped = false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (lavaSeedEquipped)
            {
                // 攻击使敌人着火（原版减益）
                target.AddBuff(BuffID.OnFire, 600); // 10秒着火效果
            }
        }
    }
}