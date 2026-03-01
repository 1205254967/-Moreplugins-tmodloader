using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Moreplugins.Content.Items.Accessories;

namespace Moreplugins.Content.Items.Accessories
{
    /// <summary>
    /// 处理血肉墙掉落的GlobalNPC
    /// </summary>
    public class WallOfFleshDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            // 血肉墙10%概率掉落Tanghulu饰品
            if (npc.type == NPCID.WallofFlesh)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tanghulu>(), 10)); // 10%概率
            }
        }
    }
}