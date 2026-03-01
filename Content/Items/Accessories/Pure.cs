using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Pure饰品 - 纯净
    /// </summary>
    internal class Pure : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Pure";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Yellow; // 黄色稀有度
            Item.value = Item.sellPrice(gold: 20); // 售价20金币
        }
        #endregion

        #region 合成配方
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(null, "Holy")      // Holy饰品
                .AddIngredient(ItemID.BrokenHeroSword, 1) // 断裂英雄剑
                .AddTile(TileID.MythrilAnvil)               // 秘银砧/山铜砧合成
                .Register();
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 标记饰品已装备
            player.GetModPlayer<PurePlayer>().pureEquipped = true;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion
    }

    /// <summary>
    /// Pure饰品的玩家类
    /// </summary>
    public class PurePlayer : ModPlayer
    {
        public bool pureEquipped; // 饰品是否装备

        public override void ResetEffects()
        {
            pureEquipped = false;
        }

        public override void UpdateEquips()
        {
            if (pureEquipped)
            {
                // 增加2最大仆从数量
                Player.maxMinions += 2;

                // 召唤物获得15%乘算伤害加成
                Player.GetDamage(DamageClass.Summon) *= 1.05f;

                // 获得8点防御
                Player.statDefense += 8;

                // 获得8点护甲穿透
                Player.GetArmorPenetration(DamageClass.Summon) += 8;
            }
        }
    }
}