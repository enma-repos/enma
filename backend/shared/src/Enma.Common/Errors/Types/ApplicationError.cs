using FluentResults;

namespace Enma.Common.Errors.Types;

public abstract class ApplicationError : Error
{
    public string Code { get; }
    public string Category { get; }

    protected ApplicationError(string message, string code, string category)
        : base(message)
    {
        Code = code;
        Category = category;

        WithMetadata(nameof(Code), Code);
        WithMetadata(nameof(Category), Category);
    }
}