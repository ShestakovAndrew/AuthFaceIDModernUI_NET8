using AuthFaceIDModernUI.Secure;
using Emgu.CV;
using Emgu.CV.Structure;
using ModernLoginWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AuthFaceIDModernUI.DataBase
{
    public class UsersDataBase
    {
        private UsersContext usersContext;

        public UsersDataBase()
        {
            usersContext = new UsersContext();
        }

        public bool IsLoginExistInDB(string userLogin)
        {
            try
            {
                return usersContext.Users.Any(u => u.Login == userLogin);
            }
            catch 
            { 
                return false; 
            }
        }

        public bool AddNewUser(string login, string password)
        {
            usersContext.Add(new User { Login = login, Password = PasswordHasher.Hash(password) });
            usersContext.SaveChanges();
            return true;
        }

        public bool IsPasswordCorrectForUser(string userLogin, string password)
        {
            User? userFromDB = GetUserFromDBByLogin(userLogin);

            if (userFromDB != null)
            {
                return PasswordHasher.Verify(userFromDB.Password, password);
            }

            return false;
        }

        public bool ChangeUserPassword(string userLogin, string password)
        {
            User? userToChange = GetUserFromDBByLogin(userLogin);

            if (userToChange != null) 
            {
                userToChange.Password = PasswordHasher.Hash(password);
                usersContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool SaveFaceByLogin(Mat userFace, string userLogin)
        {
            User? userToChange = GetUserFromDBByLogin(userLogin);

            if (userToChange != null) 
            {
                userToChange.faceID = userFace.ToImage<Bgr, byte>().ToJpegData();
                userToChange.isExistFaceID = true;
                usersContext.SaveChanges();
                return true;
            }

            return false;
        }

        private User? GetUserFromDBByLogin(string userLogin)
        {
            return usersContext.Users.Where(u => u.Login == userLogin).First();
        }
    }
}
