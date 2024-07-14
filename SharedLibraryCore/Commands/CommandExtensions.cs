using SharedLibraryCore.Interfaces;

namespace SharedLibraryCore.Commands
{
    public static class CommandExtensions
    {
        public static bool IsTargetingSelf(this GameEvent gameEvent)
        {
            return gameEvent.Origin?.Equals(gameEvent.Target) ?? false;
        }

        public static bool CanPerformActionOnTarget(this GameEvent gameEvent)
        {
            return gameEvent.Origin?.Level > gameEvent.Target?.Level;
        }
        
        /// <summary>
        /// determines the command configuration name for given manager command
        /// </summary>
        /// <param name="command">command to determine config name for</param>
        /// <returns></returns>
        public static string CommandConfigNameForType(this IManagerCommand command)
        {
            return command.GetType().Name is "ScriptCommand"
                ? $"{char.ToUpper(command.Name[0])}{command.Name[1..]}Command"
                : command.GetType().Name;
        }
    }
}
