namespace AfterFrame.Domain.Users;

public sealed class User
{
    private User()
    {
    }

    private User(Guid id, string displayName, DateTimeOffset createdAtUtc)
    {
        Id = id;
        DisplayName = displayName;
        CreatedAtUtc = createdAtUtc;
    }

    public Guid Id { get; private set; }

    public string DisplayName { get; private set; } = string.Empty;

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public static User Create(
        string displayName,
        DateTimeOffset createdAtUtc)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException("Display name is required.", nameof(displayName));
        }

        return new User(Guid.CreateVersion7(), displayName.Trim(), createdAtUtc);
    }
}