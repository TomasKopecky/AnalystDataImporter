using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace AnalystDataImporter.Services
{
    public static class ServiceLocator
    {
        public static IServiceProvider ServiceProvider;

        public static void SetLocatorProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public static IServiceProvider Current => ServiceProvider;

        public static T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }
    }

}
