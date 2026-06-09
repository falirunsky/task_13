namespace task_12.Services
{
    public interface IDialogService
    {
        void ShowInfo(string message);
        void ShowWarning(string message);
        void ShowError(string message);
        bool ShowConfirmation(string message);
    }
}
