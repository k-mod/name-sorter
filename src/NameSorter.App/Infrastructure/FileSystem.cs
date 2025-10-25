using System.Collections.Generic;
using System.IO;
using NameSorter.App.Abstractions;

namespace NameSorter.App.Infrastructure;

internal class FileSystem : IFileSystem
{
    public bool FileExists(string path) => File.Exists(path);

    public TextReader OpenRead(string path) => File.OpenText(path);

    public void WriteAllLines(string path, IEnumerable<string> lines) => File.WriteAllLines(path, lines);

    public string GetCurrentDirectory() => Directory.GetCurrentDirectory();
}
