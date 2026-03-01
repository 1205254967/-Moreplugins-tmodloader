using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// NuclearWarhead饰品 - 双头核弹
    /// </summary>
    internal class NuclearWarhead : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/NuclearWarhead";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Red; // 红色稀有度
            Item.value = Item.sellPrice(gold: 50); // 售价50金币
        }
        #endregion

        #region 合成配方
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ShroomiteBar, 10)      // 10个蘑菇矿锭
                .AddIngredient(ItemID.RocketI, 100)          // 100个火箭一型
                .AddIngredient(ItemID.RocketI, 100)          // 100个火箭一型
                .AddIngredient(ItemID.AdhesiveBandage, 1)     // 1个粘性绷带
                .AddTile(TileID.TinkerersWorkbench)         // 工匠作坊合成
                .Register();
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 标记饰品已装备
            player.GetModPlayer<NuclearWarheadPlayer>().nuclearWarheadEquipped = true;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion
    }

    /// <summary>
    /// NuclearWarhead饰品的玩家类
    /// </summary>
    public class NuclearWarheadPlayer : ModPlayer
    {
        public bool nuclearWarheadEquipped; // 饰品是否装备
        private int nukeTimer;

        public override void ResetEffects()
        {
            nuclearWarheadEquipped = false;
        }

        public override void PostUpdate()
        {
            if (nuclearWarheadEquipped)
            {
                nukeTimer++;
                if (nukeTimer >= 180)//30秒
                {
                    SpawnFirstNuke();
                    nukeTimer = 0;
                }
            }
        }

        private void SpawnFirstNuke()
        {
            // 寻找附近的敌人
            NPC target = FindNearestEnemy();
            if (target != null)
            {
                // 计算发射方向（雪人大炮攻击模板）
                Vector2 direction = Vector2.Normalize(target.Center - Player.Center);
                float speed = 10f; // 发射速度
                
                // 发射第一颗火箭（火箭一型）
                Projectile nuke1 = Projectile.NewProjectileDirect(
                    Player.GetSource_Accessory(Player.HeldItem),
                    Player.Center,
                    direction * speed,
                    ProjectileID.RocketI, // 火箭一型
                    1, // 最小伤害，确保OnHitNPC触发
                    0f, // 无击退
                    Player.whoAmI
                );
                // 设置火箭属性（雪人大炮风格）
                nuke1.tileCollide = false; // 不破坏物块
                nuke1.friendly = true; // 对敌人造成伤害
                nuke1.hostile = false; // 不对玩家造成伤害
                nuke1.owner = Player.whoAmI; // 设置所有者
                nuke1.maxPenetrate = 1; // 只穿透一次，确保爆炸触发
                nuke1.penetrate = 1; // 只穿透一次，确保爆炸触发
                nuke1.usesLocalNPCImmunity = true;
                nuke1.localNPCHitCooldown = -1;
                nuke1.timeLeft = 3600; // 60秒
                nuke1.netUpdate = true;
                // 增加爆炸半径为3倍
                nuke1.scale = 3f;
                
                // 设置强追踪效果（完全模拟雪人大炮，持续追踪移动的敌怪）
                nuke1.ai[0] = target.whoAmI; // 目标NPC的ID
                nuke1.ai[1] = 1f; // 启用强追踪模式
                // 设置标记，用于识别这是核弹饰品发射的导弹
                nuke1.ai[2] = 1f; // 使用ai[2]作为标记
                nuke1.netUpdate = true;
            }
        }

        // 移除OnHitNPC方法，改为使用全局projectile钩子


        private NPC FindNearestEnemy()
        {
            NPC target = null;
            float minDistance = float.MaxValue;
            float searchRadius = 1000f; // 搜索半径

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.CanBeChasedBy() && !npc.friendly)
                {
                    float distance = Vector2.Distance(Player.Center, npc.Center);
                    if (distance < searchRadius && distance < minDistance)
                    {
                        minDistance = distance;
                        target = npc;
                    }
                }
            }

            return target;
        }
    }
}