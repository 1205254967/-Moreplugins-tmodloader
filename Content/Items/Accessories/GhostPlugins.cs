using Terraria;
using Terraria.ID;
using Moreplugins.Content.Players;


namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Ghost饰品 - 幽灵
    /// </summary>
    internal class GhostPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 20);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.GetModPlayer<PluginsPlayer>().ghostEquipped = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SpectreBar, 10)             // 10个幽灵锭
                .AddTile(TileID.MythrilAnvil)                    // 山铜/秘银砧合成
                .Register();
        }
    }
}