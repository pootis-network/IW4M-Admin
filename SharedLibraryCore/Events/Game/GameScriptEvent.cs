using System;
using System.Reflection;
using SharedLibraryCore.Interfaces.Events;

namespace SharedLibraryCore.Events.Game;

public class GameScriptEvent : GameEventV2, IGameScriptEvent
{
    public string ScriptData { get; set; }
    public string EventName { get; } = null;

    public virtual void ParseArguments()
    {
        var arguments = ScriptData.Split(';', StringSplitOptions.RemoveEmptyEntries);

        var propIndex = 0;
        foreach (var argument in arguments)
        {
            var parts = argument.Split(['='], 2);
            PropertyInfo propertyInfo = null;
            string rawValue;

            if (parts.Length == 2) // handle as key/value pairs
            {
                var propertyName = parts[0].Trim();
                rawValue = parts[1].Trim();
                propertyInfo = GetType().GetProperty(propertyName,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly);
            }
            else
            {
                rawValue = argument;

                try
                {
                    propertyInfo =
                        GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase |
                                                BindingFlags.DeclaredOnly)[
                            propIndex];
                }
                catch
                {
                    // ignored
                }
            }

            if (propertyInfo is null)
            {
                continue;
            }

            try
            {
                var method = propertyInfo.PropertyType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public,
                    [typeof(string)]);

                var convertedValue = method is not null
                    ? method!.Invoke(null, [rawValue])!
                    : Convert.ChangeType(rawValue, propertyInfo.PropertyType);

                propertyInfo.SetValue(this, convertedValue);
            }
            catch (TargetInvocationException ex) when (ex.InnerException is FormatException &&
                                                       propertyInfo.PropertyType == typeof(bool))
            {
                propertyInfo.SetValue(this, rawValue != "0");
            }

            catch
            {
                // ignored
            }

            propIndex++;
        }
    }
}
