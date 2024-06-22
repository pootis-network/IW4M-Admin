using Data.Models;
using SharedLibraryCore;
using IW4MAdmin.Plugins.LiveRadar.Events;

// ReSharper disable CompareOfFloatsByEqualityOperator
#pragma warning disable CS0659

namespace IW4MAdmin.Plugins.LiveRadar;

public class RadarDto
{
    public string Name { get; set; }
    public long Guid { get; set; }
    public Vector3 Location { get; set; }
    public Vector3 ViewAngles { get; set; }
    public string Team { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Score { get; set; }
    public int PlayTime { get; set; }
    public string Weapon { get; set; }
    public int Health { get; set; }
    public bool IsAlive { get; set; }
    public Vector3 RadianAngles => new Vector3(ViewAngles.X.ToRadians(), ViewAngles.Y.ToRadians(), ViewAngles.Z.ToRadians());
    public int Id => GetHashCode();

    public override bool Equals(object obj)
    {
        if (obj is RadarDto re)
        {
            return re.ViewAngles.X == ViewAngles.X &&
                   re.ViewAngles.Y == ViewAngles.Y &&
                   re.ViewAngles.Z == ViewAngles.Z &&
                   re.Location.X == Location.X &&
                   re.Location.Y == Location.Y &&
                   re.Location.Z == Location.Z;
        }

        return false;
    }

    public static RadarDto FromScriptEvent(LiveRadarScriptEvent scriptEvent, long generatedBotGuid)
    {
        var parsedEvent = new RadarDto
        {
            Guid = generatedBotGuid,
            Location = scriptEvent.Location,
            ViewAngles = scriptEvent.ViewAngles.FixIW4Angles(),
            Team = scriptEvent.Team,
            Kills = scriptEvent.Kills,
            Deaths = scriptEvent.Deaths,
            Score = scriptEvent.Score,
            Weapon =scriptEvent.Weapon,
            Health = scriptEvent.Health,
            IsAlive = scriptEvent.IsAlive,
            PlayTime = scriptEvent.PlayTime
        };

        return parsedEvent;
    }
}
