using Microsoft.Extensions.DependencyInjection;
using NameSorter.App.Abstractions;
using NameSorter.App.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameSorter.App.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNameSorterApp(this IServiceCollection services)
        {
            services.AddSingleton<IApp, NameSorterApp>();
            services.AddSingleton<INameSortingService, NameSortingService>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<IConsoleWriter, ConsoleWriter>();
            return services;
        }
    }
}
