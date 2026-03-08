using System.Security;
using Terraria.Localization;

namespace Moreplugins.Core.Utilities
{
    public static partial class MPUtils
    {
        public static string ToLanValue(this string langPath) => Language.GetTextValue(langPath);
        public static string ToPercent(this float floatValue) => $"{floatValue * 100f}%";
    }
}
