using System;
using Data.Abstractions;
using Data.Models.Server;
using Microsoft.Extensions.DependencyInjection;
using SharedLibraryCore;
using SharedLibraryCore.Configuration;
using SharedLibraryCore.Interfaces;

namespace IW4MAdmin.Application.Factories
{
    /// <summary>
    /// implementation of IGameServerInstanceFactory
    /// </summary>
    /// <param name="translationLookup"></param>
    /// <param name="metaService"></param>
    /// <param name="serviceProvider"></param>
    internal class GameServerInstanceFactory(
        ITranslationLookup translationLookup,
        IMetaServiceV2 metaService,
        IServiceProvider serviceProvider)
        : IGameServerInstanceFactory
    {
        /// <summary>
        /// creates an IW4MServer instance
        /// </summary>
        /// <param name="config">server configuration</param>
        /// <param name="manager">application manager</param>
        /// <returns></returns>
        public Server CreateServer(ServerConfiguration config, IManager manager)
        {
            return new IW4MServer(config,
                serviceProvider.GetRequiredService<CommandConfiguration>(), translationLookup, metaService,
                serviceProvider, serviceProvider.GetRequiredService<IClientNoticeMessageFormatter>(),
                serviceProvider.GetRequiredService<ILookupCache<EFServer>>());
        }
    }
}
