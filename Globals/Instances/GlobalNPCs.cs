using Moreplugins.Content.GlobalNPCs;
using Moreplugins.Content.Items.Accessories;
using Moreplugins.Content.Pets;
using Moreplugins.Globals.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Globals.Instances
{
    public class PluginGlobalNPC : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            switch(shop.NpcType)
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
                //全体恶魔眼掉落迪斯科棱镜
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
                    npcLoot.AddLootCommon<EyePlugins>(10);
                    break;
                //手，
                case NPCID.HallowBoss:
                    npcLoot.Add(ItemDropRule.ByCondition(new DaytimeCondition(), ItemType<HandPlugins>(), 1, 1, 1));
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
                    npcLoot.AddLootCommon<DiscoPlugins>(10);
                    break;

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
                //别特么用硬编码我草
                //所有这种要显示给玩家的内容必须得统一用本地化
                //return "在白天击败光之女皇时掉落";

                //这里使用了封装的拓展方法
                //具体语义可以跳转一下
                return $"Mods.Moreplugins.PluginConditoins.OnDaytimeEmpress".ToLanValue();
            }
        }

    }
}
