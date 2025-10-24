using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NameSorter.App.Abstractions;
using NameSorter.App.Extensions;
using NameSorter.App.Infrastructure;

namespace NameSorter.App;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddNameSorterApp();
        using var host = builder.Build();

        var app = host.Services.GetRequiredService<IApp>();
        return await app.RunAsync(args, CancellationToken.None);
    }
}
