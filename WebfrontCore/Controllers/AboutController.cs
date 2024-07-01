using System.Linq;
using IW4MAdmin.WebfrontCore.Extensions;
using IW4MAdmin.WebfrontCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using SharedLibraryCore;
using SharedLibraryCore.Configuration;
using SharedLibraryCore.Interfaces;

namespace IW4MAdmin.WebfrontCore.Controllers
{
    public class AboutController : BaseController
    {
        private readonly ApplicationConfiguration _appConfig;

        public AboutController(IManager manager, ApplicationConfiguration appConfig) : base(manager)
        {
            _appConfig = appConfig;
        }

        public IActionResult Index()
        {
            ViewBag.Description = Localization["WEBFRONT_ABOUT_DESCRIPTION"].FormatExt(
                _appConfig.ShouldUseFallbackBranding()
                    ? _appConfig.WebfrontCustomBranding
                    : _appConfig.CommunityInformation.Name);
            ViewBag.Title = _appConfig.ShouldUseFallbackBranding()
                ? Localization["WEBFRONT_NAV_ABOUT"]
                : _appConfig.CommunityInformation.Name;

            var activeServers = _appConfig.Servers.Where(server =>
                Manager.GetServers().FirstOrDefault(s => s.ListenAddress == server.IPAddress && s.ListenPort == server.Port) != null);

            var info = new CommunityInfo
            {
                GlobalRules = _appConfig.GlobalRules,
                ServerRules = activeServers.ToDictionary(
                    config =>
                    {
                        var server = Manager.GetServers().First(server =>
                            server.ListenAddress == config.IPAddress && server.ListenPort == config.Port);
                        return (server.ServerName, server.EndPoint);
                    },
                    config => config.Rules),
                CommunityInformation = _appConfig.CommunityInformation
            };

            return View(info);
        }
    }
}
