using System.Collections.Generic;
using SharedLibraryCore;

namespace IW4MAdmin.Plugins.ProfanityDeterment;

public class ProfanityDetermentSettings
{
    public List<string> OffensiveWords { get; set; } =
    [
        @"(ph|f)[a@]g[s\$]?",
        @"(ph|f)[a@]gg[i1]ng",
        @"(ph|f)[a@]gg?[o0][t\+][s\$]?",
        @"(ph|f)[a@]gg[s\$]",
        @"(ph|f)[e3][l1][l1]?[a@][t\+][i1][o0]",
        @"(ph|f)u(c|k|ck|q)",
        @"(ph|f)u(c|k|ck|q)[s\$]?",
        @"(c|k|ck|q)un[t\+][l1][i1](c|k|ck|q)",
        @"(c|k|ck|q)un[t\+][l1][i1](c|k|ck|q)[e3]r",
        @"(c|k|ck|q)un[t\+][l1][i1](c|k|ck|q)[i1]ng",
        @"b[i1][t\+]ch[s\$]?",
        @"b[i1][t\+]ch[e3]r[s\$]?",
        @"b[i1][t\+]ch[e3][s\$]",
        @"b[i1][t\+]ch[i1]ng?",
        @"n[i1]gg?[e3]r[s\$]?",
        @"[s\$]h[i1][t\+][s\$]?",
        @"[s\$][l1]u[t\+][s\$]?"
    ];

    public bool EnableProfanityDeterment { get; set; } =
        Utilities.CurrentLocalization.LocalizationIndex["PLUGINS_PROFANITY_SETUP_ENABLE"].PromptBool();

    public string ProfanityWarningMessage { get; set; } =
        Utilities.CurrentLocalization.LocalizationIndex["PLUGINS_PROFANITY_WARNMSG"];

    public string ProfanityKickMessage { get; set; } =
        Utilities.CurrentLocalization.LocalizationIndex["PLUGINS_PROFANITY_KICKMSG"];

    public int KickAfterInfringementCount { get; set; } = 2;
    public bool KickOnInfringingName { get; set; } = true;
}
