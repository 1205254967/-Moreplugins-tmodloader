using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace Moreplugins.Content.GlobalNPCs
{
    /// <summary>
    /// 迪斯科棱晶的掉落
    /// </summary>
    public class DiscoDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            // 让月球领主掉落迪斯科棱晶
            if (npc.type == NPCID.MoonLordCore)
            {
                // 添加掉落项，掉落概率为10%
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.Accessories.DiscoPlugins>(), 10, 1, 1));
            }
        }
    }
}