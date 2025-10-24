using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NameSorter.App.Abstractions;

namespace NameSorter.App;

internal sealed class NameSorterApp(INameSortingService sortingService, IFileSystem fileSystem, IConsoleWriter console) : IApp
{
    private const string OutputFileName = "sorted-names-list.txt";

    private enum ExitCode
    {
        Success = 0,
        InvalidArguments = 1,
        InputFileNotFound = 2,
        InvalidName = 3,
        FileIoError = 4
    }

    private readonly INameSortingService _sortingService = sortingService;
    private readonly IFileSystem _fileSystem = fileSystem;
    private readonly IConsoleWriter _console = console;

    public Task<int> RunAsync(string[] args, CancellationToken cancellationToken)
    {
        if (args.Length != 1)
        {
            _console.WriteError("Usage: name-sorter <path-to-unsorted-names-list.txt>");
            return Task.FromResult((int)ExitCode.InvalidArguments);
        }

        var inputPath = args[0];

        if (!_fileSystem.FileExists(inputPath))
        {
            _console.WriteError($"The file '{inputPath}' could not be found.");
            return Task.FromResult((int)ExitCode.InputFileNotFound);
        }

        try
        {
            using var reader = _fileSystem.OpenRead(inputPath);
            var sortedNames = _sortingService.Sort(reader);

            foreach (var name in sortedNames)
            {
                _console.WriteLine(name);
            }

            var outputDirectory = _fileSystem.GetCurrentDirectory();
            var outputPath = Path.Combine(outputDirectory, OutputFileName);
            _fileSystem.WriteAllLines(outputPath, sortedNames);

            return Task.FromResult((int)ExitCode.Success);
        }
        catch (InvalidDataException ex)
        {
            _console.WriteError(ex.Message);
            return Task.FromResult((int)ExitCode.InvalidName);
        }
        catch (IOException ex)
        {
            _console.WriteError($"A file I/O error occurred: {ex.Message}");
            return Task.FromResult((int)ExitCode.FileIoError);
        }
    }
}
