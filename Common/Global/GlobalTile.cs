using Moreplugins.Content.Items.Accessories;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Common.Global
{
    internal class GlobalMushroom : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail != false || noItem != false) { return; }
            int x = i * 16;
            int y = j * 16;
            Tile tile = Main.tile[i, j];

            if (type == Terraria.ID.TileID.Plants && tile.TileFrameX == 18 * 8 && Main.rand.Next(100) == 1)
            {
                Item.NewItem(new EntitySource_TileBreak(i, j) ,x, y, 16, 16, ModContent.ItemType<MushroomPlugins>(), 1);
            }
        }
    }
}