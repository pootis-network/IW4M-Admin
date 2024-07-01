using Data.Models;
using IW4MAdmin.Plugins.Stats.Client.Game;

namespace IW4MAdmin.Plugins.Stats.Client.Abstractions;

public interface IWeaponNameParser
{
    WeaponInfo Parse(string weaponName, Reference.Game gameName);
}
