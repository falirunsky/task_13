using System.Windows;

namespace task_12.Services
{
    // Зарегистрирован как Transient: без состояния, создание не затратно.
    public class DialogService : IDialogService
    {
        public void ShowInfo(string message) =>
            MessageBox.Show(message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

        public void ShowWarning(string message) =>
            MessageBox.Show(message, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);

        public void ShowError(string message) =>
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

        public bool ShowConfirmation(string message) =>
            MessageBox.Show(message, "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question)
            == MessageBoxResult.Yes;
    }
}
