using System.Collections.Generic;
using System.Linq;
using Data.Models;
using IW4MAdmin.Plugins.Stats.Client.Abstractions;
using IW4MAdmin.Plugins.Stats.Client.Game;
using IW4MAdmin.Plugins.Stats.Config;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace IW4MAdmin.Plugins.Stats.Client
{
    public class WeaponNameParser : IWeaponNameParser
    {
        private readonly ILogger _logger;
        private readonly StatsConfiguration _config;

        public WeaponNameParser(ILogger<WeaponNameParser> logger, StatsConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public WeaponInfo Parse(string weaponName, Reference.Game gameName)
        {
            var configForGame = _config.WeaponNameParserConfigurations
                ?.FirstOrDefault(config => config.Game == gameName) ?? new WeaponNameParserConfiguration()
            {
                Game = gameName
            };

            var splitWeaponName = weaponName.Split(configForGame.Delimiters);

            if (!splitWeaponName.Any())
            {
                _logger.LogError("Could not parse weapon name {Weapon}", weaponName);

                return new WeaponInfo()
                {
                    Name = "Unknown"
                };
            }

            // remove the _mp suffix
            var filtered = splitWeaponName
                .Where(part => part != configForGame.WeaponSuffix && part != configForGame.WeaponPrefix)
                .ToList();
            var baseName = filtered.First();
            var attachments = new List<string>();

            if (filtered.Count() > 1)
            {
                attachments.AddRange(filtered.Skip(1));
            }

            var weaponInfo = new WeaponInfo()
            {
                RawName = weaponName,
                Name = baseName,
                Attachments = attachments.Select(attachment => new AttachmentInfo()
                {
                    Name = attachment
                }).ToList()
            };

            return weaponInfo;
        }
    }
}
