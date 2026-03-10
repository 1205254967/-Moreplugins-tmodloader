using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Content.Items.Accessories;
// 模组命名空间严格改为 MorePlugins（符合C#命名规范，空格替换为驼峰）
// 注：C#命名空间不支持空格，此处用MorePlugins替代"More plugins"，tModLoader模组名可在build.txt中配置
namespace MorePlugins.Content.Items.Accessories
{
    // 类名唯一化：增加前缀避免全局重名，同时保留核心标识
    public class HeartofSteelPlugins : BasicPlugins
    {
        // 物品基础属性（极简写法，无冗余配置）
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 15);
            Item.lifeRegen = 2;
            base.SetDefaults();    
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.HeartLantern, 1).
                AddIngredient(ItemID.CharmofMyths, 1).
                AddIngredient(ItemID.AvengerEmblem, 1).
                AddIngredient(ItemID.HallowedBar, 5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
        // 重写UpdateAccessory：仅保留核心逻辑，无多余判断
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            // 治疗药水冷却减少（原版ExamplePotionDelayAccessory逻辑）
            player.PotionDelayModifier *= 0.75f;
            player.lifeRegen += 2;
            // 乘算5%：额外乘法叠加，符合原版机制
            player.GetDamage<GenericDamageClass>() *= 1.05f;
        }
    }
}