using Moreplugins.Content.Players;
using ReLogic.Utilities;
using System.Collections.Generic;
using Terraria;
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
            player.GetModPlayer<PluginsPlayer>().enchantedacc = true;
            int warding = 0;
            int lucky = 0;
            int menacing = 0;
            int quick = 0;
            int violent = 0;

            for (int i = 0; i < player.armor.Length; i++)
            {
                Item acc = player.armor[i];
                if (i >= 13 && i <= 19)
                {
                    continue;
                }
                if (acc.prefix == PrefixID.Warding)
                {
                    warding += 2;
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
                if (acc.prefix == PrefixID.Quick2)
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
            genericAttackSpeed += violent;
        }
    }

    public class EnchantedPlusinsGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;

            bool[] TooltipBool = [true, true, true, true, true];
            int WardingFlavorTooltipIndex = tooltips.FindIndex(line => line.Name == "PrefixAccDefense" && line.Mod == "Terraria");
            int LuckyFlavorTooltipIndex = tooltips.FindIndex(line => line.Name == "PrefixAccCritChance" && line.Mod == "Terraria");
            int MenacingFlavorTooltipIndex = tooltips.FindIndex(line => line.Name == "PrefixAccDamage" && line.Mod == "Terraria");
            int QuickFlavorTooltipIndex = tooltips.FindIndex(line => line.Name == "PrefixAccMoveSpeed" && line.Mod == "Terraria");
            int ViolentFlavorTooltipIndex = tooltips.FindIndex(line => line.Name == "PrefixAccMeleeSpeed" && line.Mod == "Terraria");
          
            string wardingvalue = Language.GetTextValue("Mods.Moreplugins.EnchantedPlusinsTooltip.Warding");
            string luckyvalue = Language.GetTextValue("Mods.Moreplugins.EnchantedPlusinsTooltip.Lucky");
            string menacingvalue = Language.GetTextValue("Mods.Moreplugins.EnchantedPlusinsTooltip.Menacing");
            string quickvalue = Language.GetTextValue("Mods.Moreplugins.EnchantedPlusinsTooltip.Quick");
            string violentvalue = Language.GetTextValue("Mods.Moreplugins.EnchantedPlusinsTooltip.Violent");
            if (player.GetModPlayer<PluginsPlayer>().enchantedacc)
            {
                for (int i = 0; i < player.armor.Length; i++)
                {

                    Item acc = player.armor[i];

                    if (acc.prefix == PrefixID.Warding)
                    {
                        TooltipLine flavorTooltip = new TooltipLine(Mod, "WardingPrefixTooltipName", wardingvalue);
                        if (WardingFlavorTooltipIndex + 1 == 0) { continue; }
                        if (TooltipBool[0]) { tooltips.Insert(WardingFlavorTooltipIndex + 1, flavorTooltip); }
                        TooltipBool[0] = false;
                        continue;
                    }
                    if (acc.prefix == PrefixID.Lucky)
                    {
                        TooltipLine flavorTooltip = new TooltipLine(Mod, "LuckyPrefixTooltipName", luckyvalue);
                        if (LuckyFlavorTooltipIndex + 1 == 0) { continue; }
                        if (TooltipBool[1]) { tooltips.Insert(LuckyFlavorTooltipIndex + 1, flavorTooltip); }
                        TooltipBool[1] = false;
                        continue;
                    }
                    if (acc.prefix == PrefixID.Menacing)
                    {

                        TooltipLine flavorTooltip = new TooltipLine(Mod, "MenacingPrefixTooltipName", menacingvalue);
                        if (MenacingFlavorTooltipIndex + 1 == 0) { continue; }
                        if (TooltipBool[2]){ tooltips.Insert(MenacingFlavorTooltipIndex + 1, flavorTooltip); }
                        TooltipBool[2] = false;
                        continue;
                    }
                    if (acc.prefix == PrefixID.Quick2)
                    {
                        TooltipLine flavorTooltip = new TooltipLine(Mod, "QuickPrefixTooltipName", quickvalue);
                        if (QuickFlavorTooltipIndex + 1 == 0) { continue; }
                        if (TooltipBool[3]) { tooltips.Insert(QuickFlavorTooltipIndex + 1, flavorTooltip); }
                        TooltipBool[3] = false;
                        continue;
                    }
                    if (acc.prefix == PrefixID.Violent)
                    {
                        TooltipLine flavorTooltip = new TooltipLine(Mod, "ViolentPrefixTooltipName", violentvalue);
                        if (ViolentFlavorTooltipIndex + 1 == 0) { continue; }
                        if (TooltipBool[4]) { tooltips.Insert(ViolentFlavorTooltipIndex + 1, flavorTooltip); }
                        TooltipBool[4] = false;
                        continue;
                    }
                }
            }
        }
    }
}