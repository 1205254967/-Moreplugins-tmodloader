using Moreplugins.Content.Tiles;
using System.Collections.Generic;
using Terraria;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Moreplugins.Content.Systems
{
    // This example shows spawning rubble tiles during world generation.
    public class PluginsWorldGen : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            // Add a GenPass immediately after the "Piles" pass. ExampleOreSystem explains this approach in more detail.
            int PilesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Piles"));

            if (PilesIndex != -1)
            {
                tasks.Insert(PilesIndex + 1, new PlusinsPilesPass("Moreplugins Mod Piles", 100f));
            }
        }
    }
    public class PlusinsPilesPass : GenPass
    {
        public PlusinsPilesPass(string name, float loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = (string)Language.GetText("Mods.Moreplugins.MapObject.EnchantedPlusinsShrine");

            int[] tileTypes = [ModContent.TileType<EnchantedPlusinsShrine>()];

            // To not be annoying, we'll only spawn 15 Example Rubble near the spawn point.
            // This example uses the Try Until Success approach: https://github.com/tModLoader/tModLoader/wiki/World-Generation#try-until-success
            for (int k = 0; k < 4; k++)
            {
                bool success = false;
                int attempts = 0;

                while (!success)
                {
                    attempts++;
                    if (attempts > 1000)
                    {
                        break;
                    }
                    int x = WorldGen.genRand.Next(Main.maxTilesX / 2 - 1800, Main.maxTilesX / 2 + 1800);
                    int y = WorldGen.genRand.Next((int)GenVars.rockLayerLow , (int)GenVars.lavaLine);
                    int tileType = WorldGen.genRand.Next(tileTypes);
                    int placeStyle = WorldGen.genRand.Next(1);

                    if (Main.tile[x, y].TileType == tileType)
                    {
                        continue;
                    }

                    WorldGen.PlaceTile(x, y, tileType, mute: true, style: placeStyle);
                    success = Main.tile[x, y].TileType == tileType;
                }
            }
        }
    }
}