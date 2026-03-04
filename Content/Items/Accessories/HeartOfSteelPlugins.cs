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
        private const float MultiplicativeDamageBoost = 1.05f;
        // 药水冷却：减少25%（原版倍率逻辑）
        private const float PotionCooldownMultiplier = 0.75f;
        // 生命恢复：每秒2点
        private const int LifeRegenPerSecond = 2;

        // 物品基础属性（极简写法，无冗余配置）
        public override void SetDefaults()
        {
            // 尺寸：原版32x32，用Vector2统一配置
            Item.Size = new Microsoft.Xna.Framework.Vector2(32);
            // 核心属性：仅保留必要项，避免无效配置触发Bug
            Item.maxStack = 1;          // 饰品不可堆叠
            Item.accessory = true;      // 标记为饰品（原版必需）
            Item.rare = ItemRarityID.Yellow; // 黄色稀有度
            Item.value = Item.sellPrice(gold: 15); // 15金币价值
        }
        public override void AddRecipes()
        {
            // 配方构建：分步写法避免编译时的隐式冲突
            var recipe = CreateRecipe();
            // 材料列表：严格按需求配置，神话护身符ID为CharmofMyths
            recipe.AddIngredient(ItemID.HeartLantern, 1);       // 红心灯笼x1
            recipe.AddIngredient(ItemID.CharmofMyths, 1);       // 神话护身符x1（修正后ID）
            recipe.AddIngredient(ItemID.AvengerEmblem, 1);      // 复仇者徽章x1
            recipe.AddIngredient(ItemID.HallowedBar, 5);        // 神圣锭x5
            // 合成平台：秘银砧（原版困难模式标准）
            recipe.AddTile(TileID.MythrilAnvil);
            // 注册配方：单独调用避免链式调用的隐式错误
            recipe.Register();
        }
        // 重写UpdateAccessory：仅保留核心逻辑，无多余判断
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            // 1. 治疗药水冷却减少（原版ExamplePotionDelayAccessory逻辑）
            player.PotionDelayModifier *= PotionCooldownMultiplier;

            // 2. 生命恢复（原版帧计数逻辑，1秒=60帧，+2等价于每秒2点）
            player.lifeRegen += LifeRegenPerSecond;

            // 乘算5%：额外乘法叠加，符合原版机制
            player.GetDamage(DamageClass.Generic) *= MultiplicativeDamageBoost;
        }
    }
}