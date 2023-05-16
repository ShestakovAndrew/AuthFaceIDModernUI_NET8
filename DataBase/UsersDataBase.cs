using AuthFaceIDModernUI.Secure;
using ModernLoginWindow;

namespace AuthFaceIDModernUI.DataBase
{
    public class UsersDataBase
    {
        private UsersContext usersContext;

        public UsersDataBase()
        {
            usersContext = new UsersContext();
        }

        public bool AddFaceByLogin(string userLogin)
        {
            User? userToChange = GetUserFromDBByLogin(userLogin);

            if (userToChange != null)
            {
                userToChange.isExistFaceID = true;
                usersContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool AddNewUser(string login, string password)
        {
            usersContext.Add(new User { Login = login, Password = PasswordHasher.Hash(password) });
            usersContext.SaveChanges();
            return true;
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

        public bool DeleteFaceByLogin(string userLogin)
        {
            User? userFromDB = GetUserFromDBByLogin(userLogin);

            if (userFromDB != null)
            {
                userFromDB.isExistFaceID = false;
                usersContext.SaveChanges();
                return true;
            }

            return false;
        }

        public int GetIDByLogin(string userLogin)
        {
            User? userFromDB = GetUserFromDBByLogin(userLogin);

            if (userFromDB != null)
            {
                return userFromDB.Id;
            }

            throw new Exception("Пользователь не найден");
        }

        public bool IsExistFaceIDByLogin(string userLogin)
        {
            User? userFromDB = GetUserFromDBByLogin(userLogin);

            if (userFromDB != null)
            {
                return userFromDB.isExistFaceID;
            }

            throw new Exception("Пользователь не найден");
        }

        public bool IsLoginExistInDB(string userLogin)
        {
            return GetUserFromDBByLogin(userLogin) != null;
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

        private User? GetUserFromDBByLogin(string userLogin)
        {
            return usersContext.Users!.Where(u => u.Login == userLogin).FirstOrDefault();
        }
    }
}