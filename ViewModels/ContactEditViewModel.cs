using System.Text.RegularExpressions;
using System.Windows.Input;
using task_12.Data;
using task_12.Services;

namespace task_12.ViewModels
{
    // [MVVM — ViewModel экрана «Редактирование контакта»]
    // Зарегистрирован как Transient.
    // Вызывается из ContactsListViewModel через NavigationService.NavigateTo<ContactEditViewModel>(vm => vm.Load(contact)).
    public class ContactEditViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly IContactRepository _repository;
        private readonly INavigationService _navigationService;

        private static readonly Regex PhoneRegex =
            new(@"^(\+7\d{10}|8\d{10}|\d{10})$", RegexOptions.Compiled);

        // Исходные данные редактируемого контакта
        private int _contactId;
        private string _name = string.Empty;
        private string _phone = string.Empty;
        private bool _isBusy;

        public ContactEditViewModel(
            IDialogService dialogService,
            IContactRepository repository,
            INavigationService navigationService)
        {
            _dialogService = dialogService;
            _repository = repository;
            _navigationService = navigationService;

            SaveCommand = new RelayCommand(_ => _ = SaveAsync());
            CancelCommand = new RelayCommand(_ => GoBack());
        }

        // Вызывается NavigationService перед показом экрана
        public void Load(Contact contact)
        {
            _contactId = contact.Id;
            Name = contact.Name;
            Phone = contact.Phone;
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }

        // Флаг занятости — блокирует повторные нажатия во время сохранения
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                _dialogService.ShowError("Имя контакта не может быть пустым.");
                return;
            }

            if (!PhoneRegex.IsMatch(Phone))
            {
                _dialogService.ShowError(
                    "Неверный формат номера телефона.\n" +
                    "Допустимые форматы: +7XXXXXXXXXX, 8XXXXXXXXXX или 10 цифр.");
                return;
            }

            // Исключаем текущий контакт из проверки дубликатов
            if (await _repository.ExistsByPhoneAsync(Phone, excludeId: _contactId))
            {
                _dialogService.ShowWarning($"Контакт с номером {Phone} уже существует.");
                return;
            }

            IsBusy = true;
            try
            {
                var updated = new Contact { Id = _contactId, Name = Name, Phone = Phone };
                await _repository.UpdateAsync(updated);
                _dialogService.ShowInfo($"Контакт «{Name}» успешно обновлён.");
                GoBack();
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка сохранения:\n{ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void GoBack() =>
            _navigationService.NavigateTo<ContactsListViewModel>();
    }
}
