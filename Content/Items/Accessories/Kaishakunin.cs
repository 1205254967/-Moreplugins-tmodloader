using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Kaishakunin饰品
    /// </summary>
    internal class MorePlugins_Kaishakunin : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Kaishakunin";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Orange; // 橙色稀有度
            Item.value = Item.sellPrice(gold: 2); // 售价2金币
        }
        #endregion

        #region 合成配方
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Muramasa, 1)       // 村正
                .AddIngredient(ItemID.GoldBar, 5)        // 5个金锭
                .AddTile(TileID.Anvils)                 // 铁砧/铅砧合成
                .Register();
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 标记饰品已装备
            player.GetModPlayer<KaishakuninPlayer>().kaishakuninEquipped = true;
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
    /// Kaishakunin饰品的玩家类
    /// </summary>
    public class KaishakuninPlayer : ModPlayer
    {
        public bool kaishakuninEquipped; // 饰品是否装备
        public int boostTimer; // 伤害提升计时器
        public bool damageBoostActive; // 伤害提升是否激活

        public override void ResetEffects()
        {
            kaishakuninEquipped = false;
        }

        public override void PreUpdate()
        {
            if (kaishakuninEquipped)
            {
                if (boostTimer > 0)
                {
                    boostTimer--;
                }
                else
                {
                    // 每5秒激活一次伤害提升
                    damageBoostActive = true;
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (damageBoostActive)
            {
                // 下一次攻击伤害提升50%
                modifiers.FinalDamage *= 1.5f;
                // 重置状态
                damageBoostActive = false;
                boostTimer = 300; // 5秒（60帧/秒 * 5秒）
            }
        }
    }
}