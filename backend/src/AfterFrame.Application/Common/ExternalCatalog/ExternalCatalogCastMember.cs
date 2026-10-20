namespace AfterFrame.Application.Common.ExternalCatalog;

public sealed record ExternalCatalogCastMember(
    string ExternalId,
    string Name,
    string? Character,
    string? ProfilePath,
    int Order);