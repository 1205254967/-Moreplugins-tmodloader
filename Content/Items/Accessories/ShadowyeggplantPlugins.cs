using Moreplugins.Content.Players;
using Moreplugins.Core.Utilities;
using Terraria;
using Terraria.ID;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Shadowyeggplant饰品
    /// </summary>
    public class ShadowyeggplantPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Purple; // 紫色稀有度
            Item.value = Item.sellPrice(gold: 4); // 售价4金币
            Item.manaIncrease = 50;
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 5)        // 5个魔矿锭
                .AddIngredient(ItemID.ShadowScale, 5)        // 5个暗影鳞片
                .AddTile(TileID.Anvils)                      // 铁砧/铅砧合成
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.MPPlayer().shadowyeggplantEquipped = true;
            // 增加1点魔力再生
            player.manaRegen += 1;
            // 跳跃速度提升，间接增加跳跃高度
            player.jumpSpeedBoost += 0.5f;
        }
    }
 }