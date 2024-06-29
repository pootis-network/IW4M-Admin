using Data.Models;
using SharedLibraryCore.Database.Models;

namespace SharedLibraryCore.Events.Management;

public class ClientPenaltyEvent : ManagementEvent
{
    public EFClient Client { get; init; }
    public EFPenalty Penalty { get; init; }
}
