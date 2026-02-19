namespace Enma.Common.Errors.Types;

public sealed class ForbiddenError : ApplicationError
{
    public ForbiddenError(string message, string code = "forbidden", string category = "forbidden") 
        : base(message, code, category) { }
}