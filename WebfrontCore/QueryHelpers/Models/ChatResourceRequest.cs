using IW4MAdmin.Plugins.Stats.Dtos;

namespace IW4MAdmin.WebfrontCore.QueryHelpers.Models;

public class ChatResourceRequest : ChatSearchQuery
{
    public bool HasData => !string.IsNullOrEmpty(MessageContains) || !string.IsNullOrEmpty(ServerId) ||
                           ClientId is not null || SentAfterDateTime is not null;
}
