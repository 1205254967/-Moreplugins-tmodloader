using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// TerraHeart饰品 - 泰拉之心
    /// </summary>
    internal class TerraHeartPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Yellow; // 金色稀有度
            Item.value = Item.sellPrice(gold: 50); // 售价50金币
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DuskPlugins>())      // Dusk饰品
                .AddIngredient(ModContent.ItemType<PurePlugins>())      // Pure饰品
                .AddIngredient(ItemID.DestroyerEmblem, 1) // 毁灭者徽章
                .AddTile(TileID.TinkerersWorkbench)               // 工匠作坊合成
                .Register();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            // 提升10点武器基础面板伤害
            player.GetDamage(DamageClass.Generic).Flat += 5f;
            // 10点防御力
            player.statDefense += 10;
            // 10点伤害减免
            player.endurance += 0.05f;
            // 乘算伤害加成15%
            player.GetDamage(DamageClass.Generic) *= 1.10f;
            // 暴击率提升10%
            player.GetCritChance(DamageClass.Generic) += 8f;
            // 3点魔力再生
            player.manaRegen += 2;
            // 3点生命再生
            player.lifeRegen += 2;
            // 50点最大生命值
            player.statLifeMax2 += 40;
            // 50点最大魔力值
            player.statManaMax2 += 40;
            // 2最大仆从数
            player.maxMinions += 2;

            // 标记饰品已装备
            player.GetModPlayer<PluginsPlayer>().terraHeartEquipped = true;
        }
    }
}