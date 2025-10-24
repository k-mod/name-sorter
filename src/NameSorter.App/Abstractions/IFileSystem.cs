using System.Collections.Generic;
using System.IO;

namespace NameSorter.App.Abstractions;

public interface IFileSystem
{
    bool FileExists(string path);

    TextReader OpenRead(string path);

    void WriteAllLines(string path, IEnumerable<string> lines);

    string GetCurrentDirectory();
}
