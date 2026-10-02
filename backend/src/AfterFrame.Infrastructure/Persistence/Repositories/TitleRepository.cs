using AfterFrame.Application.Titles;
using Microsoft.EntityFrameworkCore;

namespace AfterFrame.Infrastructure.Persistence.Repositories;

internal sealed class TitleRepository(AfterFrameDbContext dbContext) : ITitleRepository
{
    public Task<bool> ExistsAsync(Guid titleId, CancellationToken cancellationToken = default)
    {
        return dbContext.Titles.AnyAsync(title => title.Id == titleId, cancellationToken);
    }
}