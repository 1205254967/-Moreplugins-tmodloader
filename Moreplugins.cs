//Global using static了ModContent，这样不需要每次调用tmod内容的时候要多
global using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Moreplugins.Content.Items.Accessories;
using System.Dynamic;

namespace Moreplugins
{
    public class PluginsSlotExtra : Mod
    {
        public static PluginsSlotExtra Instance;
        
        public override void Load() => Instance = this;
        public override void Unload() => Instance = null;
    }

    internal class PluginsSlotExtraUpdateUI : ModSystem
    {
        private static int posX;
        private static int posY;

        internal static int PosX { get => posX; set => posX = value; }
        internal static int PosY { get => posY; set => posY = value; }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (Main.gameMenu)
                return;

            int mapH = (Main.mapEnabled && !Main.mapFullscreen && Main.mapStyle == 1) ? 256 : 0;
            Main.inventoryScale = 0.85f;

            if (Main.mapEnabled)
            {
                int adjustY = (Main.LocalPlayer.extraAccessory) ? 610 + PlayerInput.UsingGamepad.ToInt() * 30 : 600;
                mapH = ((mapH + adjustY) > Main.screenHeight) ? Main.screenHeight - adjustY : mapH;
            }
            int slotCount = ((Main.screenHeight < 900) && (7 + Main.LocalPlayer.GetAmountOfExtraAccessorySlotsToShow() >= 8)) ?
                7 : 7 + Main.LocalPlayer.GetAmountOfExtraAccessorySlotsToShow();

            PosX = Main.screenWidth - 82 - 12 - (47 * 3) - (int)(TextureAssets.InventoryBack.Width() * Main.inventoryScale);
            PosY = (int)(mapH + 174 + 4 + slotCount * 56 * Main.inventoryScale);
        }
    }

    [Autoload]
    public class PluginsSlotExtraSlot : ModAccessorySlot
    {
        public override string Name => "PluginsSlotExtra";

        public override Vector2? CustomLocation => new Vector2(PluginsSlotExtraUpdateUI.PosX, PluginsSlotExtraUpdateUI.PosY);

        public override bool CanAcceptItem(Item item, AccessorySlotType context) =>
            item.ModItem != null && item.ModItem is BasicPlugins;
        public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo) => 
            item.ModItem != null && item.ModItem is BasicPlugins;

        public override string FunctionalTexture => GetInstance<SteelPlugins>().Texture;

        public override void OnMouseHover(AccessorySlotType context)
        {
            //Main.hoverItemName = Language.GetTextValue(Mod.GetLocalizationKey($"AccessorySlot.{nameof(context)}"));
            switch (context)
            {
                case AccessorySlotType.FunctionalSlot:
                    Main.hoverItemName = Language.GetTextValue("Mods.Moreplugins.AccessorySlot.FunctionalSlot");
                    break;

                case AccessorySlotType.VanitySlot:
                    Main.hoverItemName = Language.GetTextValue("Mods.Moreplugins.AccessorySlot.VanitySlot");
                    break;

                case AccessorySlotType.DyeSlot:
                    Main.hoverItemName = Language.GetTextValue("Mods.Moreplugins.AccessorySlot.DyeSlot");
                    break;
            }
        }
    }
}