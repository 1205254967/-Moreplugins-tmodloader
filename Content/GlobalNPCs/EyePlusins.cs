using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Content.GlobalNPCs
{
    internal class EyePlusinsDrop : GlobalNPC
    {
            public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
            {
            // 让所有种类的恶魔眼都掉落迪斯科棱晶
            if (npc.type == NPCID.CataractEye || npc.type == NPCID.CataractEye2 || npc.type == NPCID.DemonEye ||
                npc.type == NPCID.DemonEye2 || npc.type == NPCID.DemonEyeOwl || npc.type == NPCID.DemonEyeSpaceship ||
                npc.type == NPCID.DialatedEye || npc.type == NPCID.DialatedEye2 || npc.type == NPCID.GreenEye ||
                npc.type == NPCID.GreenEye2 || npc.type == NPCID.PurpleEye || npc.type == NPCID.PurpleEye2 ||
                npc.type == NPCID.SleepyEye || npc.type == NPCID.SleepyEye2)
            {
                    // 添加掉落项，掉落概率为1%
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.Accessories.EyePlugins>(), 1, 1, 1));
                }
            }
        }
    }
}