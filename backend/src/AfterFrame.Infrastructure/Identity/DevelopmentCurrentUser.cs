using AfterFrame.Application.Common.Identity;

namespace AfterFrame.Infrastructure.Identity;

internal sealed class DevelopmentCurrentUser : ICurrentUser
{
    public Guid UserId => DevelopmentUserDefaults.Id;
}