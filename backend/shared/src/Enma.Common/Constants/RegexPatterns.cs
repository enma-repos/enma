using System.Text.RegularExpressions;

namespace Enma.Common.Constants;

public static partial class RegexPatterns
{
    private const string EmailPattern = @"^(?!\.)(?!.*\.\.)([A-Z0-9_%+\-]+(?:\.[A-Z0-9_%+\-]+)*)@([A-Z0-9](?:[A-Z0-9\-]{0,61}[A-Z0-9])?(?:\.[A-Z0-9](?:[A-Z0-9\-]{0,61}[A-Z0-9])?)+)$";
    private const string MigrationFileNamePattern = @"^(?<version>\d{4})_(?<name>[a-z0-9_]+)\.sql$";
    private const string ClickHouseTableIdentifierPattern = @"^[A-Za-z_][A-Za-z0-9_]*$";
    
    [GeneratedRegex(EmailPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    public static partial Regex Email();

    [GeneratedRegex(MigrationFileNamePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    public static partial Regex MigrationFileName();
    
    [GeneratedRegex(ClickHouseTableIdentifierPattern, RegexOptions.Compiled)]
    public static partial Regex ClickHouseTableIdentifier();

    private const string KebabCasePattern = @"^[a-z0-9]+(?:-[a-z0-9]+)*$";

    [GeneratedRegex(KebabCasePattern)]
    public static partial Regex KebabCase();
}