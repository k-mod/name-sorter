using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NameSorter.App;
using NameSorter.App.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NameSorter.Tests;

[TestClass]
public class NameSorterAppTests
{
    private readonly Mock<INameSortingService> _sortingService = new();
    private readonly Mock<IFileSystem> _fileSystem = new();
    private readonly Mock<IConsoleWriter> _console = new();

    private NameSorterApp CreateApp() => new(_sortingService.Object, _fileSystem.Object, _console.Object);

    [TestMethod]
    public async Task RunAsync_ReturnsOne_WhenNoArgumentsProvided()
    {
        var app = CreateApp();

        var result = await app.RunAsync([], CancellationToken.None);

        Assert.AreEqual(1, result);
        _console.Verify(console => console.WriteError(It.Is<string>(message => message.StartsWith("Usage"))), Times.Once);
    }

    [TestMethod]
    public async Task RunAsync_ReturnsTwo_WhenFileMissing()
    {
        var app = CreateApp();
        _fileSystem.Setup(fs => fs.FileExists("missing.txt")).Returns(false);

        var result = await app.RunAsync(["missing.txt"], CancellationToken.None);

        Assert.AreEqual(2, result);
        _console.Verify(console => console.WriteError("The file 'missing.txt' could not be found."), Times.Once);
        _fileSystem.Verify(fs => fs.OpenRead(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task RunAsync_WritesSortedNamesAndReturnsZero_OnSuccess()
    {
        var app = CreateApp();
        _fileSystem.Setup(fs => fs.FileExists("input.txt")).Returns(true);

        var reader = new Mock<TextReader>();
        _fileSystem.Setup(fs => fs.OpenRead("input.txt")).Returns(reader.Object);
        _fileSystem.Setup(fs => fs.GetCurrentDirectory()).Returns("C:\\output");

        _sortingService.Setup(service => service.Sort(reader.Object))
            .Returns(["Name One"]);

        var result = await app.RunAsync(["input.txt"], CancellationToken.None);

        Assert.AreEqual(0, result);
        _console.Verify(console => console.WriteLine("Name One"), Times.Once);
        _fileSystem.Verify(fs => fs.WriteAllLines(
                "C:\\output\\sorted-names-list.txt",
                It.Is<IEnumerable<string>>(lines => lines != null)),
            Times.Once);
    }

    [TestMethod]
    public async Task RunAsync_ReturnsThree_WhenInvalidDataExceptionThrown()
    {
        var app = CreateApp();
        _fileSystem.Setup(fs => fs.FileExists("input.txt")).Returns(true);

        var reader = new Mock<TextReader>();
        _fileSystem.Setup(fs => fs.OpenRead("input.txt")).Returns(reader.Object);

        _sortingService.Setup(service => service.Sort(It.IsAny<TextReader>()))
            .Throws(new InvalidDataException("bad"));

        var result = await app.RunAsync(["input.txt"], CancellationToken.None);

        Assert.AreEqual(3, result);
        _console.Verify(console => console.WriteError("bad"), Times.Once);
    }

    [TestMethod]
    public async Task RunAsync_ReturnsFour_WhenIOExceptionThrown()
    {
        var app = CreateApp();
        _fileSystem.Setup(fs => fs.FileExists("input.txt")).Returns(true);

        var reader = new Mock<TextReader>();
        _fileSystem.Setup(fs => fs.OpenRead("input.txt")).Returns(reader.Object);

        _sortingService.Setup(service => service.Sort(It.IsAny<TextReader>()))
            .Throws(new IOException("io failure"));

        var result = await app.RunAsync(["input.txt"], CancellationToken.None);

        Assert.AreEqual(4, result);
        _console.Verify(console => console.WriteError(It.Is<string>(msg => msg.Contains("io failure"))), Times.Once);
    }

}
