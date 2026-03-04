using Moreplugins.Content.Players;
using Terraria;
using Terraria.ID;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Shadowyeggplant饰品
    /// </summary>
    internal class ShadowyeggplantPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Purple; // 紫色稀有度
            Item.value = Item.sellPrice(gold: 4); // 售价4金币
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
            player.GetModPlayer<PluginsPlayer>().shadowyeggplantEquipped = true;
            // 增加50点最大魔力值
            player.statManaMax2 += 50;

            // 增加1点魔力再生
            player.manaRegen += 1;

            // 跳跃速度提升，间接增加跳跃高度
            player.jumpSpeedBoost += 0.5f;
        }
    }
}