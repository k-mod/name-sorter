namespace NameSorter.App.Abstractions;

public interface IConsoleWriter
{
    void WriteLine(string message);

    void WriteError(string message);
}
