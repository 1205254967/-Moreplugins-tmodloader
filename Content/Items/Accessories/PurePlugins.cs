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
            Item.rare = ItemRarityID.Yellow; // 黄色稀有度
            Item.value = Item.sellPrice(gold: 20); // 售价20金币
            Item.defense = 8;
            base.SetDefaults();    
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<HolyPlugins>())       // Holy饰品
                .AddIngredient(ItemID.BrokenHeroSword) // 断裂英雄剑
                .AddTile(TileID.MythrilAnvil)               // 秘银砧/山铜砧合成
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.MPPlayer().pureEquipped = true;

            // 增加2最大仆从数量
            player.maxMinions += 2;

            // 召唤物获得15%乘算伤害加成
            //请输入文本
            player.GetDamage(DamageClass.Summon) *= 1.05f;
            // 获得8点护甲穿透
            player.GetArmorPenetration(DamageClass.Summon) += 8;
        }
    }
}