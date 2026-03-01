using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Unity饰品 - 团结
    /// </summary>
    internal class Unity : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Unity";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red; // 红色稀有度
            Item.value = Item.sellPrice(gold: 100);
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            UnityPlayer modPlayer = player.GetModPlayer<UnityPlayer>();
            modPlayer.unityEquipped = true;

            // 增加2仆从栏
            player.maxMinions += 2;

            // 25点防御力
            player.statDefense += 25;
            // 15%的护甲减免
            player.endurance += 0.15f;
            // 20%的暴击率
            player.GetCritChance(DamageClass.Generic) += 20f;
            // 30%的伤害加成
            player.GetDamage(DamageClass.Generic) += 0.3f;
            // 5点魔力再生
            player.manaRegen += 5;
            // 100点最大魔力值
            player.statManaMax2 += 150;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion

        #region 合成配方
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentNebula, 5)      // 5个星云碎片
                .AddIngredient(ItemID.FragmentVortex, 5)      // 5个星璇碎片
                .AddIngredient(ItemID.FragmentStardust, 5)    // 5个星尘碎片
                .AddIngredient(ItemID.FragmentSolar, 5)       // 5个日耀碎片
                .AddTile(TileID.LunarCraftingStation)         // 远古操纵机合成
                .Register();
        }
        #endregion
    }

    /// <summary>
    /// Unity饰品的玩家类
    /// </summary>
    public class UnityPlayer : ModPlayer
    {
        public bool unityEquipped;
        private int[] starCellProjectileIds = new int[3] { -1, -1, -1 };

        public override void ResetEffects()
        {
            unityEquipped = false;
        }

        public override void PostUpdate()
        {
            if (unityEquipped)
            {
                // 检查并召唤三个星辰细胞
                for (int i = 0; i < 3; i++)
                {
                    if (starCellProjectileIds[i] == -1 || !Main.projectile[starCellProjectileIds[i]].active)
                    {
                        starCellProjectileIds[i] = SpawnStarCell();
                    }
                }
            }
            else
            {
                // 如果饰品未装备，移除所有召唤物
                for (int i = 0; i < 3; i++)
                {
                    if (starCellProjectileIds[i] != -1 && Main.projectile[starCellProjectileIds[i]].active)
                    {
                        Main.projectile[starCellProjectileIds[i]].Kill();
                    }
                    starCellProjectileIds[i] = -1;
                }
            }
        }

        private int SpawnStarCell()
        {
            // 计算基础伤害，与星辰细胞法杖相同（基础伤害28）
            int damage = (int)(28 * Player.GetDamage(DamageClass.Summon).Additive);

            // 召唤星辰细胞
            Projectile starCell = Projectile.NewProjectileDirect(
                Player.GetSource_Accessory(Player.HeldItem),
                Player.Center,
                Vector2.Zero,
                ProjectileID.StardustCellMinion,
                damage,
                0f,
                Player.whoAmI
            );

            // 设置为非召唤物，不占用召唤栏位
            starCell.minion = false;

            // 返回projectile ID
            return starCell.whoAmI;
        }
    }
}