using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Moreplugins.Content.Players;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// Massacre饰品
    /// </summary>
    internal class MorePlugins_Massacre : BasicPlugins
    {
        // 饰品纹理路径
        public override string Texture => "Moreplugins/Assets/Items/Accessories/Massacre";

        #region 基础属性配置
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true; // 标记为饰品
            Item.rare = ItemRarityID.Orange; // 橙色稀有度
            Item.value = Item.sellPrice(gold: 3); // 售价3金币
        }
        #endregion

        #region 合成配方
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 5)        // 5个猩红锭
                .AddIngredient(ItemID.TissueSample, 5)       // 5个组织样本
                .AddTile(TileID.Anvils)                      // 铁砧/铅砧合成
                .Register();
        }
        #endregion

        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 获得1点生命再生
            player.lifeRegen += 1;

            // 增加2点防御力
            player.statDefense += 2;

            // 标记饰品已装备
            player.GetModPlayer<MassacrePlayer>().massacreEquipped = true;
            player.GetModPlayer<PluginsPlayer>().SoundAcc = true;
        }
        #endregion

        #region 工具提示
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // 添加自定义提示文本
        }
        #endregion
    }

    /// <summary>
    /// Massacre饰品的玩家类
    /// </summary>
    public class MassacrePlayer : ModPlayer
    {
        public bool massacreEquipped; // 饰品是否装备
        private Dictionary<int, int> bleedingNPCs; // 存储流血的NPC和计时器

        public override void Initialize()
        {
            bleedingNPCs = new Dictionary<int, int>();
        }

        public override void ResetEffects()
        {
            massacreEquipped = false;
        }

        public override void PostUpdate()
        {
            if (massacreEquipped)
            {
                // 处理流血效果
                List<int> npcsToRemove = new List<int>();

                foreach (var kvp in bleedingNPCs)
                {
                    int npcIndex = kvp.Key;
                    int timer = kvp.Value;

                    // 每秒钟造成4点伤害
                    if (Main.GameUpdateCount % 60 == 0)
                    {
                        NPC npc = Main.npc[npcIndex];
                        if (npc.active && !npc.dontTakeDamage)
                        {
                            NPC.HitInfo hitInfo = new NPC.HitInfo();
                            hitInfo.Damage = 10;
                            npc.StrikeNPC(hitInfo);

                            // 生成红色粒子效果
                            for (int i = 0; i < 3; i++)
                            {
                                Vector2 offset = new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-20, 21));
                                Vector2 position = npc.Center + offset;
                                Vector2 velocity = new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
                                Dust dust = Dust.NewDustPerfect(position, DustID.Blood, velocity, 100, new Color(255, 0, 0), 1.5f);
                                dust.noGravity = true;
                            }
                        }
                        else
                        {
                            npcsToRemove.Add(npcIndex);
                        }
                    }

                    // 减少计时器
                    timer--;
                    if (timer <= 0)
                    {
                        npcsToRemove.Add(npcIndex);
                    }
                    else
                    {
                        bleedingNPCs[npcIndex] = timer;
                    }
                }

                // 移除已经结束流血的NPC
                foreach (int npcIndex in npcsToRemove)
                {
                    bleedingNPCs.Remove(npcIndex);
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (massacreEquipped)
            {
                // 攻击敌人后使敌人流血
                if (!bleedingNPCs.ContainsKey(target.whoAmI))
                {
                    bleedingNPCs[target.whoAmI] = 300; // 5秒（60帧/秒 * 5秒）
                }
                else
                {
                    // 重置计时器
                    bleedingNPCs[target.whoAmI] = 300;
                }
            }
        }
    }
}