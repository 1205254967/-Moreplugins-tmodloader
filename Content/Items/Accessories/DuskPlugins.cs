using Moreplugins.Content.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Dusk饰品 - 黄昏饰品
    /// </summary>
    internal class DuskPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Yellow; // 黄色稀有度
            Item.value = Item.sellPrice(gold: 15); // 售价15金币
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<NightPlugins>(), 1)       // 1个夜晚
                .AddIngredient(ItemID.BrokenHeroSword, 1)                    // 1个断裂英雄剑
                .AddIngredient(ItemID.FrostCore, 1)                         // 1个寒霜核
                .AddIngredient(ItemID.Ichor, 10)                             // 10个灵液
                .AddTile(TileID.MythrilAnvil)                               // 秘银砧/山铜砧合成
                .Register();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 提升5点武器基础面板伤害
            player.GetDamage(DamageClass.Generic).Flat += 5f;
            // 伤害提升5%
            player.GetDamage(DamageClass.Generic) += 0.05f;
            // 暴击率提升5%
            player.GetCritChance(DamageClass.Generic) += 5f;
            // 提升10%攻击速度
            player.GetAttackSpeed(DamageClass.Generic) += 0.1f;
            // 5防御力
            player.statDefense += 5;
            // 5%的伤害减免
            player.endurance += 0.05f;
            // 获得50点最大魔力值
            player.statManaMax2 += 50;
            // 2点魔力再生
            player.manaRegen += 2;
            // 2点生命再生
            player.lifeRegen += 2;
            // 免疫燃烧与着火了减益
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Burning] = true;

            // 标记饰品已装备
            base.UpdateAccessory(player, hideVisual);
            player.GetModPlayer<PluginsPlayer>().duskEquipped = true;
        }
    }
}