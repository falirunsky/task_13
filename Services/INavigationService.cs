namespace task_12.Services
{
    public interface INavigationService
    {
        // Навигация без параметров
        void NavigateTo<TViewModel>() where TViewModel : class;

        // Навигация с настройкой ViewModel перед показом (например, передача редактируемого контакта)
        void NavigateTo<TViewModel>(Action<TViewModel> configure) where TViewModel : class;
    }
}
