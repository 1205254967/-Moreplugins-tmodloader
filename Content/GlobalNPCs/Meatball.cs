using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace Moreplugins.Content.GlobalNPCs
{
    /// <summary>
    /// 肉球的掉落
    /// </summary>
    public class MeatballDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            // 让血鳗鱼掉落肉球
            if (npc.type == NPCID.BloodEelHead)
            {
                // 添加掉落项，掉落概率为5%
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.Accessories.MeatballPlugins>(), 20, 1, 1));
            }
        }
    }
}