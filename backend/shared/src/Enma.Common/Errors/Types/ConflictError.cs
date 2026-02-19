namespace Enma.Common.Errors.Types;

public sealed class ConflictError : ApplicationError
{
    public ConflictError(string message, string code = "conflict", string category = "conflict") 
        : base(message, code, category) { }
}