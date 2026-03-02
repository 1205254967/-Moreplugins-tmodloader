using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Chat;
using Moreplugins.Content.Items.Accessories;

namespace Moreplugins.Content.GlobalNPCs
{
    internal class ShopNPC : GlobalNPC
    {
        //在炸弹商人商店中贩卖雷管插件
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Demolitionist)
            {
                shop.Add<DetonatorPlugins>();
            }
        }
    }
}
