using Terraria;
using Terraria.ID;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Thorn饰品 - 荆棘饰品
    /// </summary>
    internal class ThornPlugins : BasicPlugins
    {

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Green; // 绿色稀有度
            Item.value = Item.sellPrice(gold: 2); // 售价2金币
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofLight, 5)              // 5个光明之魂
                .AddIngredient(ItemID.SoulofNight, 5)              // 5个暗影之魂
                .AddIngredient(ItemID.Cactus, 20)                  // 20个仙人掌
                .AddTile(TileID.WorkBenches)                       // 工作台合成
                .Register();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.GetModPlayer<PluginsPlayer>().thornEquipped = true;
        }
    }
}