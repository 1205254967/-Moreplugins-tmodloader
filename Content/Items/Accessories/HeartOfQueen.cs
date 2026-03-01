using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace Moreplugins.Content.Items.Accessories
{
    public class HeartOfQueen : BasicPlugins
    {
        public override string Texture => "Moreplugins/Assets/Items/Accessories/HeartOfQueen";
        #region 基础属性配置
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 1);
            Item.accessory = true;
            Item.expert = true;
        }
        #endregion
        #region 核心饰品效果
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Lighting.AddLight(player.Center, Main.DiscoColor.ToVector3() / 2);
            Lighting.AddLight(player.Center, Color.White.ToVector3() / 5);
        }

        public override void UpdateVanity(Player player)
        {
            Lighting.AddLight(player.Center, Main.DiscoColor.ToVector3() / 2);
            Lighting.AddLight(player.Center, Color.White.ToVector3() / 5);
        }
        #endregion
    }
}
