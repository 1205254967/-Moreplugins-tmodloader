using Terraria;
using Terraria.ID;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Kaishakunin饰品
    /// </summary>
    internal class KaishakuninPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Orange; // 橙色稀有度
            Item.value = Item.sellPrice(gold: 2); // 售价2金币
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Muramasa, 1)       // 村正
                .AddIngredient(ItemID.GoldBar, 5)        // 5个金锭
                .AddTile(TileID.Anvils)                 // 铁砧/铅砧合成
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.GetModPlayer<PluginsPlayer>().kaishakuninEquipped = true;
        }
    }
}