namespace SharedLibraryCore.Interfaces.Events;

public interface IGameScriptEventFactory
{
    IGameScriptEvent Create(string eventType, string logData);
}
