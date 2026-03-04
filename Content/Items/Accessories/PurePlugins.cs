using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Pure饰品 - 纯净
    /// </summary>
    internal class PurePlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Yellow; // 黄色稀有度
            Item.value = Item.sellPrice(gold: 20); // 售价20金币
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HolyPlugins>(), 1)       // Holy饰品
                .AddIngredient(ItemID.BrokenHeroSword, 1) // 断裂英雄剑
                .AddTile(TileID.MythrilAnvil)               // 秘银砧/山铜砧合成
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.GetModPlayer<PluginsPlayer>().pureEquipped = true;

            // 增加2最大仆从数量
            player.maxMinions += 2;

            // 召唤物获得15%乘算伤害加成
            player.GetDamage(DamageClass.Summon) *= 1.05f;

            // 获得8点防御
            player.statDefense += 8;

            // 获得8点护甲穿透
            player.GetArmorPenetration(DamageClass.Summon) += 8;
        }
    }
}