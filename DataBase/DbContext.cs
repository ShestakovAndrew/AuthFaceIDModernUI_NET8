using AuthFaceIDModernUI.Secure;
using Emgu.CV;
using Microsoft.EntityFrameworkCore;
using ModernLoginWindow;

namespace AuthFaceIDModernUI.DataBase
{
    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public string DbPath { get; }

        public UsersContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "users.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}