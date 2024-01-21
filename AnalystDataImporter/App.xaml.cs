using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AnalystDataImporter.Factories;
using AnalystDataImporter.Managers;
using AnalystDataImporter.Models;
using AnalystDataImporter.Services;
using AnalystDataImporter.Utilities;
using AnalystDataImporter.ViewModels;
using AnalystDataImporter.Views;
using AnalystDataImporter.WindowsWPF;
using AnalystDataImporter.WindowsWPF.SettingPagesWPF;

namespace AnalystDataImporter
{
    /// <summary>
    /// Hlavní třída aplikace, která nastavuje závislostní injekci a inicializuje hlavní okno aplikace.
    /// </summary>
    public partial class App
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureDi(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
            ServiceLocator.SetLocatorProvider(_serviceProvider);
            ServiceLocator._serviceProvider = _serviceProvider;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
        }

        /// <summary>
        /// Nastavení závislostní injekce.
        /// </summary>
        //TODO: V případě, že aplikace bude růst, tak je vhodné převést DI logiku do samostatné třídy - ServiceProvider() např.
        private void ConfigureDi(IServiceCollection services)
        {
            // Registrace továrny a manažera v kontejneru DI.
            

            services.AddSingleton<IElementViewModelFactory, ElementViewModelFactory>();
            services.AddSingleton<IElementManager, ElementManager>();
            services.AddSingleton<IRelationViewModelFactory, RelationViewModelFactory>();
            services.AddSingleton<IRelationManager, RelationManager>();
            services.AddSingleton<IMouseHandlingService, MouseHandlingService>();
            services.AddSingleton<ITableColumnViewModelFactory, TableColumnViewModelFactory>();
            
            services.AddSingleton<MouseCursorService, MouseCursorService>();
            services.AddSingleton<SharedStatesService, SharedStatesService>();
            services.AddSingleton<SharedCanvasPageItems, SharedCanvasPageItems>();

            // Registrace ViewModelů.
            services.AddSingleton<CompositeCanvasGridViewModel>();
            services.AddSingleton<GridViewModel>();

            services.AddSingleton<CanvasViewModel>();
            //services.AddTransient<CanvasViewModel>();
            services.AddTransient<ElementViewModel>();
            services.AddTransient<Element>();
            services.AddTransient<RelationViewModel>();
            services.AddTransient<Relation>();

            // Registrace Views.
            services.AddTransient<CanvasPage>();
            services.AddTransient<MainWindow>(); // Registrace hlavního okna.

            services.AddSingleton<PageImport1>();
            services.AddSingleton<PageImport2>();
            services.AddSingleton<MainPage>();

            //services.AddTransient<SettingWindow>();


            services.AddTransient<MousePositionInvokeCommandAction>();

            services.AddSingleton<IMessageService, MessageService>();

            services.AddSingleton<IMessageBoxService, MessageBoxService>();

            services.AddSingleton<CsvParserService, CsvParserService>();

            services.AddSingleton<SqliteDbService>();
            // Pokud máte služby nebo repozitáře, registrujte je zde.
            // services.AddTransient<IMyService, MyServiceImplementation>();
        }

        /// <summary>
        /// Metoda pro získání služby z kontejneru DI.
        /// </summary>
        public static T GetService<T>() => ((App)Current)._serviceProvider.GetRequiredService<T>();
    }
}