namespace task_12.ViewModels
{
    // Зарегистрирован как Transient.
    public class AboutViewModel : ViewModelBase
    {
        public string AppName     => "Телефонная книга";
        public string Version     => "Версия 4.0";
        public string Description =>
            "ЧТо-то.";
        public string Author      => "Лаб. работа 12";
        public string Technology  => "WPF · .NET 8 · EF Core 8 · SQL Server · Microsoft.Extensions.DependencyInjection";
    }
}
