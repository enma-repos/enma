using System.Text.RegularExpressions;

namespace Enma.Common.Constants;

public static partial class RegexPatterns
{
    private const string EmailPattern = @"^(?!\.)(?!.*\.\.)([A-Z0-9_%+\-]+(?:\.[A-Z0-9_%+\-]+)*)@([A-Z0-9](?:[A-Z0-9\-]{0,61}[A-Z0-9])?(?:\.[A-Z0-9](?:[A-Z0-9\-]{0,61}[A-Z0-9])?)+)$";
    
    [GeneratedRegex(EmailPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    public static partial Regex Email();
}