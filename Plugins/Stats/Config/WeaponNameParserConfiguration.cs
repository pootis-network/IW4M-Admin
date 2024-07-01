using Data.Models;

namespace IW4MAdmin.Plugins.Stats.Config
{
    public class WeaponNameParserConfiguration
    {
        public Reference.Game Game { get; set; }
        public char[] Delimiters { get; set; }
        public string WeaponSuffix { get; set; }
        public string WeaponPrefix { get; set; }
    }
}
