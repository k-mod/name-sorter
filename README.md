# Name Sorter

This solution implements a Name Sorter: read a list of personal names, sort them by last name and then by the given names, print the result to the console, and persist the sorted list to `sorted-names-list.txt`.

## Requirements

- .NET SDK 9.0.300 or later

## Building

```powershell
dotnet build
```

## Running

```powershell
dotnet run --project src/NameSorter.App  <path-to-unsorted-names-list.txt>
```

The application prints the sorted names to stdout and writes the same list to `sorted-names-list.txt` in the working directory. Invalid names (missing a last name or with more than three given names) stop execution with a clear error message.

### Architecture

- The entry point composes services via the generic host and dependency injection, with `IApp` orchestrating the workflow (`NameSorterApp`).
- `INameSortingService` encapsulates ordering concerns and can be reused or mocked in tests.
- File and console interactions are abstracted behind `IFileSystem` and `IConsoleWriter` to keep side effects isolated from business logic.

## Testing

The solution uses MSTest for unit testing:

```powershell
dotnet test
```

## Continuous Integration

An Azure DevOps pipeline definition is provided in `azure-pipelines.yml`. It restores dependencies, builds the solution, and executes the MSTest test suite.
