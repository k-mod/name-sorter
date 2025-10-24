using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NameSorter.App;

namespace NameSorter.Tests;

[TestClass]
public class NameSortingServiceTests
{
    private readonly NameSortingService _service = new();

    [TestMethod]
    public void Sort_ReturnsNamesOrderedByLastNameThenGivenNames()
    {
        var input = new[]
        {
            "Janet Parsons",
            "Vaughn Lewis",
            "Adonis Julius Archer",
            "Shelby Nathan Yoder",
            "Marin Alvarez",
            "London Lindsey",
            "Beau Tristan Bentley",
            "Leo Gardner",
            "Hunter Uriah Mathew Clarke",
            "Mikayla Lopez",
            "Frankie Conner Ritter"
        };

        using var reader = CreateReader(input);
        var result = _service.Sort(reader);

        CollectionAssert.AreEqual(
            new List<string>
            {
                "Marin Alvarez",
                "Adonis Julius Archer",
                "Beau Tristan Bentley",
                "Hunter Uriah Mathew Clarke",
                "Leo Gardner",
                "Vaughn Lewis",
                "London Lindsey",
                "Mikayla Lopez",
                "Janet Parsons",
                "Frankie Conner Ritter",
                "Shelby Nathan Yoder"
            },
            new List<string>(result));
    }

    [TestMethod]
    public void Sort_IgnoresEmptyOrWhitespaceLines()
    {
        var input = new[]
        {
            "  Marin Alvarez  ",
            string.Empty,
            "   ",
            "Adonis Julius Archer"
        };

        using var reader = CreateReader(input);
        var result = _service.Sort(reader);

        CollectionAssert.AreEqual(
            new List<string> { "Marin Alvarez", "Adonis Julius Archer" },
            new List<string>(result));
    }

    [TestMethod]
    public void Sort_ThrowsWhenNameHasTooFewParts()
    {
        var input = new[] { "Peter" };

        using var reader = CreateReader(input);
        var exception = Assert.Throws<InvalidDataException>(() => _service.Sort(reader));

        Assert.Contains("Peter", exception.Message);
    }

    [TestMethod]
    public void Sort_ThrowsWhenNameHasTooManyGivenNames()
    {
        var input = new[] { "First Second Third Fourth Last" };

        using var reader = CreateReader(input);
        Assert.Throws<InvalidDataException>(() => _service.Sort(reader));
    }

    [TestMethod]
    public void Sort_UsesCaseInsensitiveComparison()
    {
        var input = new[]
        {
            "amy Adams",
            "Amy aDams",
            "Bob Adams"
        };

        using var reader = CreateReader(input);
        var result = _service.Sort(reader);

        CollectionAssert.AreEqual(
            new List<string>
            {
                "amy Adams",
                "Amy aDams",
                "Bob Adams"
            },
            new List<string>(result));
    }

    private static StringReader CreateReader(IEnumerable<string> lines)
    {
        var content = string.Join(Environment.NewLine, lines);
        return new StringReader(content);
    }
}
