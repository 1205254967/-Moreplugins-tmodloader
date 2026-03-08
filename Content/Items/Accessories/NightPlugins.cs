using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Moreplugins.Content.Players;
using Moreplugins.Core.Utilities;
using Moreplugins.Core.GlobalInstance.Items;
using Terraria.Localization;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Night饰品 - 夜晚饰品
    /// </summary>
    public class NightPlugins : BasicPlugins
    {
        private int FlatDamageAndCrits = 5;
        private float DamageAndDR = 0.05f;
        private float SummonCrit = 0.30f;
        private float SummonCritDamage = 1.5f;
        private int MaxMana = 30;
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Orange; // 橙色稀有度
            Item.value = Item.sellPrice(gold: 5); // 售价5金币
            Item.defense = 3;
            base.SetDefaults();    
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(FlatDamageAndCrits, DamageAndDR.ToPercent(), SummonCrit.ToPercent(), SummonCritDamage.ToPercent(), MaxMana);

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<LeafPlugins>())
                .AddIngredient(ItemType<LavaSeedPlugins>())
                .AddIngredient(ItemType<KaishakuninPlugins>()) 
                .AddRecipeGroup(PluginRecipeGroup.AnyEvilPlugin)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.GetDamage<GenericDamageClass>() += FlatDamageAndCrits;
            player.GetCritChance<GenericDamageClass>() += FlatDamageAndCrits;
            player.GetDamage<GenericDamageClass>() += DamageAndDR;
            player.endurance += DamageAndDR;
            player.MPPlayer().summonCritChance =SummonCrit;
            player.MPPlayer().summonCritDamageMultipler = SummonCritDamage;
            // 最大魔力值提升30
            player.statManaMax2 += MaxMana;
            // 免疫燃烧与着火了减益
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Burning] = true;

            // 标记饰品已装备
            player.MPPlayer().nightEquipped = true;
        }
    }
}