using SharedLibraryCore;

namespace IW4MAdmin.Plugins.Welcome
{
    public class WelcomeConfiguration
    {
        public string UserAnnouncementMessage { get; set; } =
            Utilities.CurrentLocalization.LocalizationIndex["PLUGINS_WELCOME_USERANNOUNCE_V2"];

        public string UserWelcomeMessage { get; set; } =
            Utilities.CurrentLocalization.LocalizationIndex["PLUGINS_WELCOME_USERWELCOME_V2"];

        public string PrivilegedAnnouncementMessage { get; set; } =
            Utilities.CurrentLocalization.LocalizationIndex["PLUGINS_WELCOME_PRIVANNOUNCE_V2"];
    }
}
