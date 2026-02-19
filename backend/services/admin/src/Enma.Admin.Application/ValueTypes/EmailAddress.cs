using System.Text.RegularExpressions;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Admin.Application.ValueTypes;

public readonly record struct EmailAddress(string Value)
{
    private static readonly Regex Rx = new(
        @"^(?=.{5,320}$)[A-Za-z0-9.!#$%&'*+/=?^_`{|}~-]+@(?:[A-Za-z0-9-]+\.)+[A-Za-z]{2,}$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static Result<EmailAddress> Create(string? email)
    {
        email = (email ?? string.Empty).Trim();

        if (!Rx.IsMatch(email))
        {
            return Result.Fail(ApplicationErrors.Validation("Email", code: "email_format"));
        }

        return Result.Ok(new EmailAddress(email));
    }

    public override string ToString() => Value;
}