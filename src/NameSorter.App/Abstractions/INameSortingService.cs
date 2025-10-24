using System.Collections.Generic;
using System.IO;

namespace NameSorter.App.Abstractions;

public interface INameSortingService
{
    IReadOnlyList<string> Sort(TextReader reader);

}
