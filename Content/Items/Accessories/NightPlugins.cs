using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Night饰品 - 夜晚饰品
    /// </summary>
    internal class NightPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Orange; // 橙色稀有度
            Item.value = Item.sellPrice(gold: 5); // 售价5金币
        }

        public override void AddRecipes()
        {
            // 第一个合成配方：使用屠戮
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LeafPlugins>(), 1)       // 1个树叶
                .AddIngredient(ModContent.ItemType<LavaSeedPlugins>(), 1)    // 1个熔岩之心
                .AddIngredient(ModContent.ItemType<KaishakuninPlugins>(), 1) // 1个刽子手
                .AddIngredient(ModContent.ItemType<MassacrePlugins>(), 1)    // 1个屠戮
                .AddTile(TileID.DemonAltar)                                         // 恶魔祭坛合成
                .Register();

            // 第二个合成配方：使用阴暗的茄子
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LeafPlugins>(), 1)       // 1个树叶
                .AddIngredient(ModContent.ItemType<LavaSeedPlugins>(), 1)    // 1个熔岩之心
                .AddIngredient(ModContent.ItemType<KaishakuninPlugins>(), 1) // 1个刽子手
                .AddIngredient(ModContent.ItemType<ShadowyeggplantPlugins>(), 1) // 1个阴暗的茄子
                .AddTile(TileID.DemonAltar)                                         // 恶魔祭坛合成
                .Register();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            // 提升2点基础面板伤害
            player.GetDamage(DamageClass.Generic).Flat += 2f;
            // 3%伤害加成
            player.GetDamage(DamageClass.Generic) += 0.03f;
            // 暴击率提升3%
            player.GetCritChance(DamageClass.Generic) += 3f;
            // 获得3点防御
            player.statDefense += 3;
            // 3%伤害减免
            player.endurance += 0.03f;
            // 召唤物有15%概率造成150%伤害
            player.GetDamage(DamageClass.Summon) += 0.5f;
            // 最大魔力值提升30
            player.statManaMax2 += 30;
            // 免疫燃烧与着火了减益
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Burning] = true;

            // 标记饰品已装备
            player.GetModPlayer<PluginsPlayer>().nightEquipped = true;
        }
    }
}