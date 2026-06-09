using Microsoft.Extensions.DependencyInjection;
using task_12.ViewModels;

namespace task_12.Services
{
    // Зарегистрирован как Singleton: один экземпляр на всё приложение.
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<TViewModel>() where TViewModel : class
        {
            NavigateTo<TViewModel>(null!);
        }

        public void NavigateTo<TViewModel>(Action<TViewModel> configure) where TViewModel : class
        {
            var shell = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            configure?.Invoke(viewModel);
            shell.CurrentViewModel = viewModel;
        }
    }
}
