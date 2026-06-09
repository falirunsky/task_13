using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using task_12.Data;
using task_12.Services;

namespace task_12.ViewModels
{
    // [MVVM — ViewModel экрана «Список контактов»]
    // Зарегистрирован как Transient.
    // Получает IDialogService, IContactRepository и INavigationService через конструктор.
    // Про EF Core и DbContext ничего не знает — только про интерфейс репозитория.
    public class ContactsListViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly IContactRepository _repository;
        private readonly INavigationService _navigationService;

        private static readonly Regex PhoneRegex =
            new(@"^(\+7\d{10}|8\d{10}|\d{10})$", RegexOptions.Compiled);

        private string _newName = string.Empty;
        private string _newPhone = string.Empty;
        private Contact? _selectedContact;
        private bool _isLoading;

        public ContactsListViewModel(
            IDialogService dialogService,
            IContactRepository repository,
            INavigationService navigationService)
        {
            _dialogService = dialogService;
            _repository = repository;
            _navigationService = navigationService;

            Contacts = new ObservableCollection<Contact>();

            AddCommand = new RelayCommand(_ => _ = AddContactAsync());

            EditCommand = new RelayCommand(
                param => NavigateToEdit(param as Contact),
                param => param is Contact);

            DeleteCommand = new RelayCommand(
                param => _ = DeleteContactAsync(param as Contact),
                param => param is Contact);

            // Загрузка данных из БД при создании ViewModel (fire-and-forget)
            _ = LoadContactsAsync();
        }

        public ObservableCollection<Contact> Contacts { get; }

        // Флаг загрузки, можно привязать к ProgressBar или отключить кнопки
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public string NewName
        {
            get => _newName;
            set { _newName = value; OnPropertyChanged(); }
        }

        public string NewPhone
        {
            get => _newPhone;
            set { _newPhone = value; OnPropertyChanged(); }
        }

        public Contact? SelectedContact
        {
            get => _selectedContact;
            set { _selectedContact = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        // Загрузить все контакты из БД и заполнить коллекцию
        public async Task LoadContactsAsync()
        {
            IsLoading = true;
            try
            {
                var contacts = await _repository.GetAllAsync();
                Contacts.Clear();
                foreach (var c in contacts)
                    Contacts.Add(c);
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка загрузки контактов из базы данных:\n{ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task AddContactAsync()
        {
            if (string.IsNullOrWhiteSpace(NewName))
            {
                _dialogService.ShowError("Имя контакта не может быть пустым.");
                return;
            }

            if (!PhoneRegex.IsMatch(NewPhone))
            {
                _dialogService.ShowError(
                    "Неверный формат номера телефона.\n" +
                    "Допустимые форматы: +7XXXXXXXXXX, 8XXXXXXXXXX или 10 цифр.");
                return;
            }

            if (await _repository.ExistsByPhoneAsync(NewPhone))
            {
                _dialogService.ShowWarning($"Контакт с номером {NewPhone} уже существует.");
                return;
            }

            var contact = new Contact { Name = NewName, Phone = NewPhone };
            await _repository.AddAsync(contact);   // сохраняем в БД
            Contacts.Add(contact);                  // обновляем UI
            _dialogService.ShowInfo($"Контакт «{NewName}» успешно добавлен.");

            NewName = string.Empty;
            NewPhone = string.Empty;
        }

        // Передаюи контакт в ContactEditViewModel и переходим на экран редактирования
        private void NavigateToEdit(Contact? contact)
        {
            if (contact is null) return;
            _navigationService.NavigateTo<ContactEditViewModel>(vm => vm.Load(contact));
        }

        private async Task DeleteContactAsync(Contact? contact)
        {
            if (contact is null) return;

            if (_dialogService.ShowConfirmation(
                    $"Вы уверены, что хотите удалить контакт «{contact.Name}»?"))
            {
                await _repository.DeleteAsync(contact.Id);  // удаляем из БД
                Contacts.Remove(contact);                    // обновляем UI
            }
        }
    }
}
