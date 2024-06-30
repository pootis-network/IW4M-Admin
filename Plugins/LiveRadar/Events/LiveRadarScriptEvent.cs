using Data.Models;
using SharedLibraryCore.Events.Game;

namespace IW4MAdmin.Plugins.LiveRadar.Events;

public class LiveRadarScriptEvent : GameScriptEvent
{
    public string Name { get; set; }
    public Vector3 Location { get; set; }
    public Vector3 ViewAngles { get; set; }
    public string Team { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Score { get; set; }
    public string Weapon { get; set; }
    public int Health { get; set; }
    public bool IsAlive { get; set; }
    public int PlayTime { get; set; }
}
