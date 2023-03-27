using AuthFaceIDModernUI.Secure;
using Emgu.CV.Features2D;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ModernLoginWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public bool IsLoginExistInDB(string login)
        {
            return Users.Any(u => u.Login == login);
        }

        public bool AddNewUser(string login, string password)
        {
            Add(new User { Login = login, Password = PasswordHasher.Hash(password) });
            SaveChanges();
            return true;
        }

        public bool IsPasswordCorrectForUser(string login, string password)
        {
            User? userFromDB = Users.Where(u => u.Login == login).FirstOrDefault();

            if (userFromDB != null)
            {
                string passwordFromDB = userFromDB.Password;
                bool isPasswordVerify = PasswordHasher.Verify(passwordFromDB, password);
                return isPasswordVerify;
            }

            return false;
        }

        public bool ChangeUserPassword(string userLogin, string password)
        {
            var userToChange = Users.Where(u => u.Login == userLogin).First();
            userToChange.Password = PasswordHasher.Hash(password);
            SaveChanges();
            return true;
        }
    }
}