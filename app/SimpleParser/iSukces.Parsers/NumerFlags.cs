using System;

namespace iSukces.Parsers
{
    [Flags]
    public enum NumerFlags
    {
        None,
        AllowLedingSpaces = 1,
        RequireAtLeastOneLeadingSpace = 2,
        AllowUndescores = 4
    }
}