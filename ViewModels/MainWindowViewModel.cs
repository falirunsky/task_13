using System.Windows.Input;
using task_12.Services;

namespace task_12.ViewModels
{
    // [MVVM — Shell ViewModel]
    // Зарегистрирован как Singleton.
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private object? _currentViewModel;

        public MainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateToContactsCommand = new RelayCommand(_ => _navigationService.NavigateTo<ContactsListViewModel>());
            NavigateToAboutCommand    = new RelayCommand(_ => _navigationService.NavigateTo<AboutViewModel>());
        }

        public object? CurrentViewModel
        {
            get => _currentViewModel;
            set { _currentViewModel = value; OnPropertyChanged(); }
        }

        public ICommand NavigateToContactsCommand { get; }
        public ICommand NavigateToAboutCommand    { get; }
    }
}
