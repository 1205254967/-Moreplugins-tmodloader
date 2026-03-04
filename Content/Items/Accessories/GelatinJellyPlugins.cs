using Terraria;
using Terraria.ID;


namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// GelatinJelly饰品 - 明胶果冻
    /// </summary>
    internal class GelatinJellyPlugins : BasicPlugins
    {

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 10);
        }
        #endregion

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            // 提升10%移动速度
            player.moveSpeed += 0.1f;
            // 提升10%冲刺速度
            player.maxRunSpeed += 0.1f;
            // 提升10%飞行时间
            player.wingTimeMax = (int)(player.wingTimeMax * 1.1f);
            // 提升10%飞行速度
            player.maxFallSpeed += 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.QueenSlimeCrystal, 1)
                .AddIngredient(ItemID.IronBar, 3)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}