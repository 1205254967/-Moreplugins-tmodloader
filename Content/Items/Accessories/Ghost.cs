using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Moreplugins.Content.Players;


namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Ghost饰品 - 幽灵
    /// </summary>
    internal class Ghost : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Ghost";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 20);
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GhostPlayer>().ghostEquipped = true;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion

        #region 合成配方
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SpectreBar, 10)             // 10个幽灵锭
                .AddTile(TileID.MythrilAnvil)                    // 山铜/秘银砧合成
                .Register();
        }
        #endregion
    }

    /// <summary>
    /// Ghost饰品的玩家类
    /// </summary>
    public class GhostPlayer : ModPlayer
    {
        public bool ghostEquipped;

        public override void ResetEffects()
        {
            ghostEquipped = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (ghostEquipped)
            {
                // 回复造成伤害的1%
                int healAmount = (int)(damageDone * 0.01f);
                if (healAmount > 0)
                {
                    Player.Heal(healAmount);
                }
            }
        }
    }
}