using Microsoft.EntityFrameworkCore;

namespace task_12.Data
{
    public partial class PhoneBookDbContext : DbContext
    {
        public PhoneBookDbContext(DbContextOptions<PhoneBookDbContext> options)
            : base(options) { }

        public virtual DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }
        // позволяет дополнять конфигурацию модели без правки сгенерированного файла
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
