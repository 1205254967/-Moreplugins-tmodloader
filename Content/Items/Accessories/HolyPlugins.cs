using Moreplugins.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Holy饰品 - 神圣饰品
    /// </summary>
    public class HolyPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
            Item.defense = 3;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 3)       // 3个神圣锭
                .AddIngredient(ItemID.Ruby, 1)              // 1个红宝石
                .AddTile(TileID.MythrilAnvil)               // 秘银砧/山铜砧合成
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            // 1最大仆从数
            player.maxMinions += 1;
            // 5点护甲穿透
            player.GetArmorPenetration(DamageClass.Summon) += 5;
            player.MPPlayer().holyPluginsEquipped = true;
        }
    }
}