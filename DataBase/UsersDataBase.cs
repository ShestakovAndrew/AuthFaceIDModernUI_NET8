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

        public bool AddNewUser(string login, string password)
        {
            usersContext.Add(new User { Login = login, Password = PasswordHasher.Hash(password), SecretWord = "" });
            usersContext.SaveChanges();
            return true;
        }

        public bool IsUserHasSecretWord(string userLogin)
        {
            User? userToChange = GetUserFromDBByLogin(userLogin);

            if (userToChange != null)
            {
                return userToChange.SecretWord != "";
            }

            throw new Exception("Пользователь не найден");
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

            throw new Exception("Пользователь не найден");
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

        public string GetLoginByID(int userID)
        {
            User? userFromDB = GetUserFromDBByID(userID);

            if (userFromDB != null)
            {
                return userFromDB.Login;
            }

            throw new Exception("Пользователь не найден");
        }

        public string GetSecretWordByID(int userID)
        {
            User? userFromDB = GetUserFromDBByID(userID);

            if (userFromDB != null)
            {
                return userFromDB.SecretWord!;
            }

            throw new Exception("Пользователь не найден");
        }

        public void DeleteUserByID(int userID)
        {
            User? userFromDB = GetUserFromDBByID(userID);

            if (userFromDB != null)
            {
                usersContext.Users!.Remove(userFromDB);
                usersContext.SaveChanges();
                return;
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

            throw new Exception("Пользователь не найден");
        }

        public void AddSecretWordForUser(string userLogin, string secretWord)
        {
            User? userFromDB = GetUserFromDBByLogin(userLogin);

            if (userFromDB != null)
            {
                userFromDB.SecretWord = secretWord;
                usersContext.SaveChanges();
                return;
            }

            throw new Exception("Пользователь не найден");
        }

        private User? GetUserFromDBByLogin(string userLogin)
        {
            return usersContext.Users!.Where(u => u.Login == userLogin).FirstOrDefault();
        }

        private User? GetUserFromDBByID(int userID)
        {
            return usersContext.Users!.Where(u => u.Id == userID).FirstOrDefault();
        }
    }
}