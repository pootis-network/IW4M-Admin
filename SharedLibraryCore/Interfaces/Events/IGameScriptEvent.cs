namespace SharedLibraryCore.Interfaces.Events;

public interface IGameScriptEvent
{
    string ScriptData { get; set; }
    string EventName { get; }
    void ParseArguments();
}
