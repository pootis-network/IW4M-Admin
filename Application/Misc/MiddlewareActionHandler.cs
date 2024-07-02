using SharedLibraryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace IW4MAdmin.Application.Misc
{
    internal class MiddlewareActionHandler(ILogger<MiddlewareActionHandler> logger) : IMiddlewareActionHandler
    {
        private readonly Dictionary<string, IList<object>> _actions = new();
        private readonly ILogger _logger = logger;

        /// <summary>
        /// Executes the action with the given name
        /// </summary>
        /// <typeparam name="T">Execution return type</typeparam>
        /// <param name="value">Input value</param>
        /// <param name="name">Name of action to execute</param>
        /// <returns></returns>
        public async Task<T> Execute<T>(T value, string name = null)
        {
            var key = string.IsNullOrEmpty(name) ? typeof(T).ToString() : name;

            if (!_actions.TryGetValue(key, out var action1)) return value;
            foreach (var action in action1)
            {
                try
                {
                    value = await ((IMiddlewareAction<T>)action).Invoke(value);
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "Failed to invoke middleware action {Name}", name);
                }
            }

            return value;
        }

        /// <summary>
        /// Registers an action by name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionType">Action type specifier</param>
        /// <param name="action">Action to perform</param>
        /// <param name="name">Name of action</param>
        public void Register<T>(T actionType, IMiddlewareAction<T> action, string name = null)
        {
            var key = string.IsNullOrEmpty(name) ? typeof(T).ToString() : name;

            if (_actions.TryGetValue(key, out var action1))
            {
                action1.Add(action);
            }
            else
            {
                _actions.Add(key, [action]);
            }
        }
    }
}
