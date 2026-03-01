using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Shadowyeggplant饰品
    /// </summary>
    internal class MorePlugins_Shadowyeggplant : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Shadowyeggplant";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Purple; // 紫色稀有度
            Item.value = Item.sellPrice(gold: 4); // 售价4金币
        }
        #endregion

        #region 合成配方
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 5)        // 5个魔矿锭
                .AddIngredient(ItemID.ShadowScale, 5)        // 5个暗影鳞片
                .AddTile(TileID.Anvils)                      // 铁砧/铅砧合成
                .Register();
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 增加50点最大魔力值
            player.statManaMax2 += 50;

            // 增加1点魔力再生
            player.manaRegen += 1;

            // 跳跃速度提升，间接增加跳跃高度
            player.jumpSpeedBoost += 0.5f;
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
    /// Shadowyeggplant饰品的玩家类
    /// </summary>
    public class ShadowyeggplantPlayer : ModPlayer
    {
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            // 10%概率造成200%伤害
            if (Main.rand.NextBool(10))
            {
                modifiers.FinalDamage *= 2f;
            }
        }
    }
}