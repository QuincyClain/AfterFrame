namespace AfterFrame.Application.Titles;

public interface ITitleRepository
{
    Task<bool> ExistsAsync(Guid titleId, CancellationToken cancellationToken = default);
}