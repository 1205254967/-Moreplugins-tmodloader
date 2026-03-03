using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace Moreplugins.Content.GlobalNPCs
{
    /// <summary>
    /// 巨人之拳的掉落
    /// </summary>
    public class GiantfistDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            // 让石巨人的左拳和右拳掉落巨人之拳
            if (npc.type == NPCID.GolemFistLeft || npc.type == NPCID.GolemFistRight)
            {
                // 添加掉落项，掉落概率为1%
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.Accessories.GiantfistPlugins>(), 100, 1, 1));
            }
        }
    }
}