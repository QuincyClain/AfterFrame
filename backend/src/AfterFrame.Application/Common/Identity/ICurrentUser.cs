namespace AfterFrame.Application.Common.Identity;

public interface ICurrentUser
{
    Guid UserId { get; }
}