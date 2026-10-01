namespace AfterFrame.Application.Common.Exceptions;

public sealed class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityName, object key)
        : base($"{entityName} '{key}' was not found.")
    {
    }
}