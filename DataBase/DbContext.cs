using Microsoft.EntityFrameworkCore;
using ModernLoginWindow;

namespace AuthFaceIDModernUI.DataBase
{
    public class UsersContext : DbContext
    {
        public DbSet<User>? Users { get; set; }
        public string DbPath { get; }

        public UsersContext()
        {
            DbPath = Config.DataBasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}