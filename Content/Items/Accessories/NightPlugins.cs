using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Core.Utilities;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Night饰品 - 夜晚饰品
    /// </summary>
    public class NightPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            base.SetDefaults(); 
            Item.rare = ItemRarityID.Orange; // 橙色稀有度
            Item.value = Item.sellPrice(gold: 5); // 售价5金币
            Item.manaIncrease = 30;
            Item.defense = 3;
        }

        public override void AddRecipes()
        {
            // 第一个合成配方：使用屠戮
            CreateRecipe()
                .AddIngredient(ItemType<LeafPlugins>(), 1)       // 1个树叶
                .AddIngredient(ItemType<LavaSeedPlugins>(), 1)    // 1个熔岩之心
                .AddIngredient(ItemType<KaishakuninPlugins>(), 1) // 1个刽子手
                .AddIngredient(ItemType<MassacrePlugins>(), 1)    // 1个屠戮
                .AddTile(TileID.DemonAltar)                                         // 恶魔祭坛合成
                .Register();

            // 第二个合成配方：使用阴暗的茄子
            CreateRecipe()
                .AddIngredient(ItemType<LeafPlugins>(), 1)       // 1个树叶
                .AddIngredient(ItemType<LavaSeedPlugins>(), 1)    // 1个熔岩之心
                .AddIngredient(ItemType<KaishakuninPlugins>(), 1) // 1个刽子手
                .AddIngredient(ItemType<ShadowyeggplantPlugins>(), 1) // 1个阴暗的茄子
                .AddTile(TileID.DemonAltar)                                         // 恶魔祭坛合成
                .Register();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.GetDamage(DamageClass.Generic).Flat += 2f;
            player.GetDamage(DamageClass.Generic) += 3 / 100f;
            player.GetCritChance(DamageClass.Generic) += 3f;
            player.endurance += 0.03f;
            player.GetDamage(DamageClass.Summon) += 0.5f;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Burning] = true;
            player.MPPlayer().nightEquipped = true;
        }
    }
}