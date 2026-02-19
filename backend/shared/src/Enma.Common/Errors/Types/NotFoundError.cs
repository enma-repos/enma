namespace Enma.Common.Errors.Types;

public sealed class NotFoundError : ApplicationError
{
    public NotFoundError(string message, string code = "not_found", string category = "not_found") 
        : base(message, code, category) { }
}