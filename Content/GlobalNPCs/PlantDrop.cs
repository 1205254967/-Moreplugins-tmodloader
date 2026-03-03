using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Moreplugins.Content.Items.Accessories;

namespace Moreplugins.Content.GlobalNPCs
{
    /// <summary>
    /// 处理世纪之花掉落的GlobalNPC
    /// </summary>
    public class BudGlobalNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            // 世纪之花10%概率掉落Bud饰品
            if (npc.type == NPCID.Plantera)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BudPlugins>(), 10)); // 10%概率
            }
        }
    }
}