using System;
using System.Collections.Generic;

namespace NameSorter.App;

internal sealed class NameRecordComparer : IComparer<NameRecord>
{
    private static readonly StringComparer Comparer = StringComparer.OrdinalIgnoreCase;

    public int Compare(NameRecord? x, NameRecord? y)
    {
        if (ReferenceEquals(x, y))
        {
            return 0;
        }

        if (x is null)
        {
            return -1;
        }

        if (y is null)
        {
            return 1;
        }

        var lastNameComparison = Comparer.Compare(x.LastName, y.LastName);

        if (lastNameComparison != 0)
        {
            return lastNameComparison;
        }

        var maxGivenNames = Math.Max(x.GivenNames.Count, y.GivenNames.Count);

        for (var index = 0; index < maxGivenNames; index++)
        {
            var xGivenName = index < x.GivenNames.Count ? x.GivenNames[index] : string.Empty;
            var yGivenName = index < y.GivenNames.Count ? y.GivenNames[index] : string.Empty;

            var comparison = Comparer.Compare(xGivenName, yGivenName);

            if (comparison != 0)
            {
                return comparison;
            }
        }

        return 0;
    }
}
