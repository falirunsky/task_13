using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using task_12.Data;
using task_12.Services;
using task_12.ViewModels;
using task_12.Views;

namespace task_12
{
    public partial class App : Application
    {
        private const string ConnectionString =
            "Server=localhost,1433;" +
            "Database=PhoneBookDB;" +
            "User Id=sa;" +
            "Password=Secret_123;" +
            "TrustServerCertificate=True;";

        private IServiceProvider _serviceProvider = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            var mainVm = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = mainVm;

            var nav = _serviceProvider.GetRequiredService<INavigationService>();
            nav.NavigateTo<ContactsListViewModel>();

            mainWindow.Show();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Transient гарантирует, что каждый репозиторий получит свой экземпляр контекста
            // и не будет конфликтов при параллельных async-операциях
            services.AddDbContext<PhoneBookDbContext>(
                options => options.UseSqlServer(ConnectionString),
                ServiceLifetime.Transient);

            services.AddTransient<IDialogService, DialogService>();
            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<ContactsListViewModel>();
            services.AddTransient<ContactEditViewModel>();   // <-- новый экран редактирования
            services.AddTransient<AboutViewModel>();
            services.AddSingleton<MainWindow>();
        }
    }
}