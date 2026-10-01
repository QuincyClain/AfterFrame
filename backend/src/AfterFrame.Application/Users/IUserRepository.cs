namespace AfterFrame.Application.Users;

public interface IUserRepository
{
    Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken = default);
}