using task_12.Data;

namespace task_12.Services
{
    // [Service — репозиторий, интерфейс]
    // Абстракция над хранилищем контактов. ViewModel работает только с этим интерфейсом
    // и не знает, откуда приходят данные (БД, файл, mock для тестов).
    public interface IContactRepository
    {
        Task<List<Contact>> GetAllAsync();
        Task AddAsync(Contact contact);
        Task UpdateAsync(Contact contact);
        Task DeleteAsync(int id);
        Task<bool> ExistsByPhoneAsync(string phone, int? excludeId = null);
    }
}
