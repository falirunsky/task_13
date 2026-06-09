using Microsoft.EntityFrameworkCore;
using task_12.Data;

namespace task_12.Services
{
    // [Service — репозиторий, реализация]
    // Зарегистрирован как Transient: время жизни совпадает с PhoneBookDbContext (тоже Transient).
    // Инкапсулирует все обращения к EF Core — ViewModel про EF не знает.
    public class ContactRepository : IContactRepository
    {
        private readonly PhoneBookDbContext _context;

        public ContactRepository(PhoneBookDbContext context)
        {
            _context = context;
        }

        // Загрузить все контакты из таблицы Contacts
        public async Task<List<Contact>> GetAllAsync() =>
            await _context.Contacts.ToListAsync();

        // Добавить новый контакт и сохранить изменения
        public async Task AddAsync(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
        }

        // Обновить существующий контакт и сохранить изменения
        public async Task UpdateAsync(Contact contact)
        {
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
        }

        // Удалить контакт по Id и сохранить изменения
        public async Task DeleteAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact is null) return;
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
        }

        // Проверить существование номера телефона (для защиты от дубликатов).
        // excludeId позволяет исключить текущий контакт при редактировании.
        public async Task<bool> ExistsByPhoneAsync(string phone, int? excludeId = null) =>
            await _context.Contacts.AnyAsync(c => c.Phone == phone && c.Id != excludeId);
    }
}
