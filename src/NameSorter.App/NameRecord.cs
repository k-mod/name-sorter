using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NameSorter.App;

internal sealed class NameRecord
{
    private readonly string[] _givenNames;

    private NameRecord(string[] givenNames, string lastName)
    {
        _givenNames = givenNames;
        LastName = lastName;
    }

    public IReadOnlyList<string> GivenNames => _givenNames;

    public string LastName { get; }

    public static NameRecord Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidDataException("Encountered an empty name while sorting.");
        }

        var parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2)
        {
            throw new InvalidDataException(
                $"'{value}' is not a valid name. A name must contain at least one given name and a last name.");
        }

        if (parts.Length > 4)
        {
            throw new InvalidDataException(
                $"'{value}' is not a valid name. A name can contain at most three given names.");
        }

        var givenNames = parts[..^1];
        var lastName = parts[^1];

        return new NameRecord(givenNames, lastName);
    }

    public override string ToString() => string.Join(' ', _givenNames.Append(LastName));
}
