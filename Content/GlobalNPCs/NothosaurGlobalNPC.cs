using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace Moreplugins.Content.GlobalNPCs
{
    /// <summary>
    /// 敌怪掉落配置
    /// </summary>
    public class NothosaurGlobalNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            // 猪龙鱼公爵掉落
            if (npc.type == NPCID.DukeFishron)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<NothosaurPlugins>(), 10)); // 10%掉率
            }
        }
    }
}