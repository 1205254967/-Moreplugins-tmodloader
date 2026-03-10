using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Content.Pets.Steel
{
	public class SteelPlusinsPetItem : ModItem
	{
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.ZephyrFish);

			Item.shoot = ProjectileType<SteelPlusinsPetProjectile>();
			Item.buffType = BuffType<SteelPlusinsPetBuff>();
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(gold: 3);
        }

		public override bool? UseItem(Player player) {
			if (player.whoAmI == Main.myPlayer) {
				player.AddBuff(Item.buffType, 3600);
			}
			return true;
		}
 	}
}