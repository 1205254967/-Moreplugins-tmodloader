using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Tanghulu饰品 - 冰糖葫芦
    /// </summary>
    internal class Tanghulu : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Tanghulu";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Red; // 红色稀有度
            Item.value = Item.sellPrice(gold: 25); // 售价25金币
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 标记饰品已装备
            player.GetModPlayer<TanghuluPlayer>().tanghuluEquipped = true;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion
    }

    /// <summary>
    /// Tanghulu饰品的玩家类
    /// </summary>
    public class TanghuluPlayer : ModPlayer
    {
        public bool tanghuluEquipped; // 饰品是否装备

        public override void ResetEffects()
        {
            tanghuluEquipped = false;
        }

        public override void UpdateEquips()
        {
            if (tanghuluEquipped)
            {
                // 最大生命值提升400
                Player.statLifeMax2 += 400;

                // 防御力减半
                Player.statDefense /= 2;
            }
        }
    }
}