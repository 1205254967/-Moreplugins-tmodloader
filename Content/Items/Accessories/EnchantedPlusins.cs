using Moreplugins.Content.Players;
using System.Collections.Generic;
using System.Threading.Channels;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
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

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            for (int i = 0; i < player.armor.Length; i++)
            {
                Item acc = player.armor[i];
                if (acc.prefix == PrefixID.Warding)
                {

                    // 这块加
                    continue;
                }
                if (acc.prefix == PrefixID.Lucky)
                {

                    continue;
                }
                if (acc.prefix == PrefixID.Menacing)
                {

                    continue;
                }
                if (acc.prefix == PrefixID.Quick)
                {

                    continue;
                }
                if (acc.prefix == PrefixID.Violent)
                {

                    continue;
                }
            }
        }
    }
}