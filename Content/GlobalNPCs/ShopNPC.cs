using Moreplugins.Content.Items.Accessories;
using Moreplugins.Content.Pets;
using Terraria.ID;
using Terraria.ModLoader;

namespace Moreplugins.Content.GlobalNPCs
{
    internal class ShopNPC : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            //在炸弹商人商店中贩卖雷管插件
            if (shop.NpcType == NPCID.Demolitionist)
            {
                shop.Add<DetonatorPlugins>();
            }
            //商人中贩卖插件宠物
            if (shop.NpcType == NPCID.Merchant)
            {
                shop.Add<PlusinsPetItem>();
            }
        }
    }
}
