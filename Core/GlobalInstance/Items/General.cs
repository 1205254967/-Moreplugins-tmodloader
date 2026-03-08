using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Moreplugins.Core.GlobalInstance.Items
{
    public partial class PluginsGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
    }
}
