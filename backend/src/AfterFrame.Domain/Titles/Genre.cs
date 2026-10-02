namespace AfterFrame.Domain.Titles;

public sealed class Genre
{
    private Genre()
    {
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public static Genre Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Genre name is required.", nameof(name));
        }

        return new Genre
        {
            Id = Guid.CreateVersion7(),
            Name = name.Trim()
        };
    }
}