namespace Enma.Common.Errors.Types;

public sealed class ValidationError : ApplicationError
{
    public ValidationError(string message, string code = "validation", string ctg = "validation") 
        : base(message, code, ctg) { }
}