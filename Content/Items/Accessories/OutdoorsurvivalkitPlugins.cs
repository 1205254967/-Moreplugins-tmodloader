using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Outdoorsurvivalkit饰品 - 户外生存装置
    /// </summary>
    internal class OutdoorsurvivalkitPlugins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Pink; // 粉色稀有度
            Item.value = Item.sellPrice(gold: 10); // 售价10金币
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofFright, 1)      // 1个恐惧之魂
                .AddIngredient(ItemID.SoulofSight, 1)       // 1个视域之魂
                .AddIngredient(ItemID.SoulofMight, 1)       // 1个力量之魂
                .AddIngredient(ItemID.SkeletronPrimePetItem, 1) // 机械骷髅王的大师模式奖励宠物
                .AddIngredient(ItemID.TwinsPetItem, 1)      // 双子魔眼的大师模式奖励宠物
                .AddIngredient(ItemID.DestroyerPetItem, 1)   // 毁灭者的大师模式奖励宠物
                .AddIngredient(ItemID.Wire, 15)             // 15根电线
                .AddIngredient(ItemID.IronBar, 20)          // 20个铁锭
                .AddTile(TileID.MythrilAnvil)               // 秘银砧/山铜砧合成
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.GetModPlayer<PluginsPlayer>().kitEquipped = true;
        }
    }
}