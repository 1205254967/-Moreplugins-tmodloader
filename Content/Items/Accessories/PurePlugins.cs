using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Content.Players;
using Moreplugins.Core.Utilities;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Pure饰品 - 纯净
    /// </summary>
    public class PurePlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 20);
            Item.defense = 8;
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
            player.MPPlayer().pureEquipped = true;
            player.maxMinions += 2;
            player.GetDamage(DamageClass.Summon) *= 1.05f;
            // 获得8点护甲穿透
            player.GetArmorPenetration(DamageClass.Summon) += 8;
        }
    }
}