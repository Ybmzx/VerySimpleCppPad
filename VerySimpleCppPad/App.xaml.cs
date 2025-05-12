using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using VerySimpleCppPad.Services;
using VerySimpleCppPad.ViewModels;

namespace VerySimpleCppPad
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();
        }

        public new static App Current => (App)Application.Current;

        private static IServiceProvider Services;


        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowVM>();
            services.AddSingleton<ISettingService, XmlSettingService>(x => new XmlSettingService("conf.xml"));
            services.AddSingleton<IMessageBoxService, MessageBoxService>();
            services.AddSingleton<IProgramFileManageService, ProgramFileManageService>(x => new ProgramFileManageService("src", "template.cpp"));
            services.AddSingleton<ICppCompileAndRunService, CppCompileAndRunService>();

            return services.BuildServiceProvider();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = Services.GetService<MainWindow>();
            mainWindow!.Show();
        }
    }
}
