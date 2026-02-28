namespace Enma.Common.Errors.Types;

public sealed class UnauthorizedError : ApplicationError
{
    public UnauthorizedError(string message, string code = "unauthorized", string category = "unauthorized")
        : base(message, code, category) { }
}

