namespace AfterFrame.Infrastructure.Identity;

internal static class DevelopmentUserDefaults
{
    internal static readonly Guid Id = Guid.Parse("0199f3f4-7c00-7000-8000-000000000001");

    internal const string DisplayName = "Demo User";

    internal static readonly DateTimeOffset CreatedAtUtc = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
}