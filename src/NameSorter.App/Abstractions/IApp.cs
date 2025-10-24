using System.Threading;
using System.Threading.Tasks;

namespace NameSorter.App.Abstractions;

public interface IApp
{
    Task<int> RunAsync(string[] args, CancellationToken cancellationToken);
}
