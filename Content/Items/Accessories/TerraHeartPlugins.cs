using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Content.Players;
using Moreplugins.Core.Utilities;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// TerraHeart饰品 - 泰拉之心
    /// </summary>
    internal class TerraHeartPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow; // 金色稀有度
            Item.value = Item.sellPrice(gold: 50); // 售价50金币
            Item.defense = 10; // 10点基础防御力
            Item.manaIncrease = 50;
            Item.lifeRegen = 2;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<DuskPlugins>())      // Dusk饰品
                .AddIngredient(ItemType<PurePlugins>())      // Pure饰品
                .AddIngredient(ItemID.DestroyerEmblem, 1) // 毁灭者徽章
                .AddTile(TileID.TinkerersWorkbench)               // 工匠作坊合成
                .Register();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.GetDamage(DamageClass.Generic).Flat += 5f;
            player.endurance += 0.05f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.1f;
            player.GetDamage(DamageClass.Generic) *= 1.10f;
            player.GetCritChance(DamageClass.Generic) += 8f;
            player.manaRegen += 2;
            player.statLifeMax2 += 50;
            player.maxMinions += 2;
            player.MPPlayer().terraHeartEquipped = true;
        }
    }
}