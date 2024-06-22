using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SharedLibraryCore.Interfaces.Events;

namespace IW4MAdmin.Application.Factories;

public class GameScriptEventFactory : IGameScriptEventFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, Type> _gameScriptEventMap;

    public GameScriptEventFactory(IServiceProvider serviceProvider, IEnumerable<IGameScriptEvent> gameScriptEventTypes)
    {
        _serviceProvider = serviceProvider;

        _gameScriptEventMap = gameScriptEventTypes
            .ToLookup(kvp => kvp.EventName ?? kvp.GetType().Name.Replace("ScriptEvent", ""))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.First().GetType());
    }

    public IGameScriptEvent Create(string eventType, string logData)
    {
        if (string.IsNullOrEmpty(eventType) || !_gameScriptEventMap.TryGetValue(eventType, out var matchedType))
        {
            return null;
        }
        
        var newEvent = _serviceProvider.GetRequiredService(matchedType) as IGameScriptEvent;

        if (newEvent is not null)
        {
            newEvent.ScriptData = logData;
        }

        return newEvent;
    }
}
