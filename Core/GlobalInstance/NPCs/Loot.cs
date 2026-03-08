using Moreplugins.Content.Items.Accessories;
using Moreplugins.Content.Pets;
using Moreplugins.Core.DropCondition;
using Moreplugins.Core.Utilities;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Core.GlobalInstance.NPCs
{
    public partial class PluginGlobalNPC : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            switch (shop.NpcType)
            {
                case NPCID.Demolitionist:
                    shop.Add<DetonatorPlugins>();
                    break;
                case NPCID.Merchant:
                    shop.Add<PlusinsPetItem>();
                    break;
            }
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            //是的这里直接用的switch管的
            switch (npc.type)
            {
                case NPCID.CataractEye:
                case NPCID.CataractEye2:
                case NPCID.DemonEye:
                case NPCID.DemonEye2:
                case NPCID.DemonEyeOwl:
                case NPCID.DemonEyeSpaceship:
                case NPCID.DialatedEye:
                case NPCID.DialatedEye2:
                case NPCID.GreenEye:
                case NPCID.GreenEye2:
                case NPCID.PurpleEye:
                case NPCID.PurpleEye2:
                case NPCID.SleepyEye:
                case NPCID.SleepyEye2:
                    npcLoot.AddLootCommon<EyePlugins>(100);
                    break;
                //手，
                case NPCID.HallowBoss:
                    npcLoot.Add(ItemDropRule.ByCondition(new DaytimeCondition(), ItemType<HandPlugins>(), 1, 1, 1));
                    npcLoot.AddLootCommon<HeartOfQueenPlugins>(1);
                    break;
                case NPCID.Plantera:
                    npcLoot.AddLootCommon<BudPlugins>(10);
                    break;
                case NPCID.WallofFlesh:
                    npcLoot.AddLootCommon<TanghuluPlugins>(10);
                    break;
                case NPCID.DukeFishron:
                    npcLoot.AddLootCommon<NothosaurPlugins>(10);
                    break;
                case NPCID.BloodEelHead:
                    npcLoot.AddLootCommon<MeatballPlugins>(20);
                    break;
                case NPCID.GolemFistLeft:
                case NPCID.GolemFistRight:
                    npcLoot.AddLootCommon<GiantfistPlugins>(100);
                    break;
                case NPCID.MoonLordCore:
                    npcLoot.AddLootCommon<DiscoPlugins>(1);
                    break;
                case NPCID.Pumpking:
                    npcLoot.AddLootCommon<PumpkinCandlePlugins>(5);
                    break;
                case NPCID.GingerbreadMan:
                    npcLoot.AddLootCommon<GingerbreadmanPlugins>(20);
                    break;
            }
        }
    }
}
