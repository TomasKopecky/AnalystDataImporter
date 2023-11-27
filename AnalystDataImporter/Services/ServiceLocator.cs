using Microsoft.Extensions.DependencyInjection;
using System;


namespace AnalystDataImporter.Services
{
    public static class ServiceLocator
    {
        public static IServiceProvider _serviceProvider;

        public static void SetLocatorProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static IServiceProvider Current => _serviceProvider;

        public static T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }

}
