using Moreplugins.Content.Items.Accessories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Moreplugins.Core.GlobalInstance.Items
{
    public class PluginRecipeGroup : ModSystem
    {
        public static string AnyEvilPlugin;
        public override void AddRecipeGroups()
        {
            AnyEvilPlugin = CreateRecipeGroup(nameof(AnyEvilPlugin), ItemType<ShadowyeggplantPlugins>(), ItemType<MassacrePlugins>());
        }
        public static string CreateRecipeGroup(string name, params int[] AllItem)
        {
            Func<string> getName = () => Language.GetTextValue("LegacyMisc.37") + " " + Lang.GetItemNameValue(AllItem[0]);
            RecipeGroup rec = new RecipeGroup(getName, AllItem);
            string realName = "Plugins:" + name;
            RecipeGroup.RegisterGroup(realName, rec);
            return realName;
        }

    }
}
