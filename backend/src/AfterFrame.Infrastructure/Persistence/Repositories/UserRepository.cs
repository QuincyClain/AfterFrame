using AfterFrame.Application.Users;
using Microsoft.EntityFrameworkCore;

namespace AfterFrame.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository(AfterFrameDbContext dbContext) : IUserRepository
{
    public Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return dbContext.Users.AnyAsync(user => user.Id == userId, cancellationToken);
    }
}