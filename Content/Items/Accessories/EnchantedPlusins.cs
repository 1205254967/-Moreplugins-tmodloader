using Moreplugins.Content.Players;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Channels;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Moreplugins.Content.Items.Accessories
{
    internal class EnchantedPlusins : BasicPlugins
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 20);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.GetModPlayer<EnchantedPlusinsPlayer>().enchantedacc = true;
            int warding = 0;
            int lucky = 0;
            int menacing = 0;
            int quick = 0;
            int violent = 0;

            for (int i = 0; i < player.armor.Length; i++)
            {
                Item acc = player.armor[i];
                if (acc.prefix == PrefixID.Warding)
                {
                    warding += 2;
                    // 这块加
                    continue;
                }
                if (acc.prefix == PrefixID.Lucky)
                {
                    lucky += 2;
                    continue;
                }
                if (acc.prefix == PrefixID.Menacing)
                {
                    menacing += 2;
                    continue;
                }
                if (acc.prefix == PrefixID.Quick)
                {
                    quick += 2;
                    continue;
                }
                if (acc.prefix == PrefixID.Violent)
                {
                    violent += 2;
                    continue;
                }
            }
            player.statDefense += warding;

            ref float genericCrit = ref player.GetCritChance<GenericDamageClass>();
            genericCrit += lucky;

            ref StatModifier genericDamage = ref player.GetDamage<GenericDamageClass>();
            genericDamage += menacing / 100f;

            player.moveSpeed += quick / 100f;

            ref float genericAttackSpeed = ref player.GetAttackSpeed<GenericDamageClass>();
            genericAttackSpeed += lucky;
        }
    }

    public class EnchantedPlusinsPlayer : ModPlayer
    {
        public bool enchantedacc;
        public override void ResetEffects()
        {
            enchantedacc = false;
        }
    }

    public class EnchantedPlusinsGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;

            int WardingFlavorTooltipIndex = tooltips.FindIndex(line => line.Name == "PrefixAccDefense" && line.Mod == "Terraria");
            int LuckyFlavorTooltipIndex = tooltips.FindIndex(line => line.Name == "PrefixAccCritChance" && line.Mod == "Terraria");
            int MenacingFlavorTooltipIndex = tooltips.FindIndex(line => line.Name == "PrefixAccDamage" && line.Mod == "Terraria");
            int QuickFlavorTooltipIndex = tooltips.FindIndex(line => line.Name == "PrefixAccMoveSpeed" && line.Mod == "Terraria");
            int ViolentFlavorTooltipIndex = tooltips.FindIndex(line => line.Name == "PrefixAccMeleeSpeed" && line.Mod == "Terraria");

            string wardingvalue = Language.GetTextValue("（+2 防御力）");
            string luckyvalue = Language.GetTextValue("（+2% 暴击率）");
            string menacingvalue = Language.GetTextValue("（+2% 伤害）");
            string quickvalue = Language.GetTextValue("（+2% 移动速度）");
            string violentvalue = Language.GetTextValue("（+2% 攻速）");

            for (int i = 0; i < player.armor.Length; i++)
            {
                
                Item acc = player.armor[i];


                if (acc.prefix == PrefixID.Warding)
                {
                    TooltipLine flavorTooltip = new TooltipLine(Mod, "WardingPrefixTooltipName", wardingvalue);
                    tooltips.Insert(WardingFlavorTooltipIndex + 1, flavorTooltip);
                    continue;
                }
                if (acc.prefix == PrefixID.Lucky)
                {
                    TooltipLine flavorTooltip = new TooltipLine(Mod, "LuckyPrefixTooltipName", luckyvalue);
                    tooltips.Insert(LuckyFlavorTooltipIndex + 1, flavorTooltip);
                    continue;
                }
                if (acc.prefix == PrefixID.Menacing)
                {

                    TooltipLine flavorTooltip = new TooltipLine(Mod, "MenacingPrefixTooltipName", menacingvalue);
                    tooltips.Insert(MenacingFlavorTooltipIndex + 1, flavorTooltip);
                    continue;
                }
                if (acc.prefix == PrefixID.Quick)
                {
                    TooltipLine flavorTooltip = new TooltipLine(Mod, "QuickPrefixTooltipName", luckyvalue);
                    tooltips.Insert(QuickFlavorTooltipIndex + 1, flavorTooltip);
                    continue;
                }
                if (acc.prefix == PrefixID.Violent)
                {
                    TooltipLine flavorTooltip = new TooltipLine(Mod, "ViolentPrefixTooltipName", violentvalue);
                    tooltips.Insert(ViolentFlavorTooltipIndex + 1, flavorTooltip);
                    continue;
                }
            }
        }
    }
}