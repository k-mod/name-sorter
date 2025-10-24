using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NameSorter.App.Abstractions;

namespace NameSorter.App;

public sealed class NameSortingService : INameSortingService
{
    private static readonly NameRecordComparer RecordComparer = new();

    public IReadOnlyList<string> Sort(TextReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        var records = new List<NameRecord>();

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            records.Add(NameRecord.Parse(line.Trim()));
        }

        return SortRecords(records);
    }

    private static IReadOnlyList<string> SortRecords(List<NameRecord> records) =>
        records
            .OrderBy(record => record, RecordComparer)
            .Select(record => record.ToString())
            .ToList();
}
