using System;
using NameSorter.App.Abstractions;

namespace NameSorter.App.Infrastructure;

internal class ConsoleWriter : IConsoleWriter
{
    public void WriteLine(string message) => Console.Out.WriteLine(message);

    public void WriteError(string message) => Console.Error.WriteLine(message);
}
