using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace Moreplugins.Content.GlobalNPCs
{
    /// <summary>
    /// 手的掉落
    /// </summary>
    public class HandDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            // 让光之女皇掉落手饰品（参考Terraprisma的掉落机制）
            if (npc.type == 636)
            {
                // 创建自定义条件类来检测白天
                var daytimeCondition = new DaytimeCondition();
                // 添加条件掉落规则，只有在白天击败光之女皇时才掉落
                npcLoot.Add(ItemDropRule.ByCondition(daytimeCondition, ModContent.ItemType<Content.Items.Accessories.HandPlugins>(), 2, 1, 1));
            }
        }
    }

    /// <summary>
    /// 检测白天的条件类
    /// </summary>
    public class DaytimeCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return Main.dayTime;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return "在白天击败光之女皇时掉落";
        }
    }
}