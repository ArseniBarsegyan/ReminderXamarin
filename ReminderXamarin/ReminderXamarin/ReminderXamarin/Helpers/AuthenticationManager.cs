using System.Linq;
using ReminderXamarin.Models;

namespace ReminderXamarin.Helpers
{
    /// <summary>
    /// Class for test purposes. Implements authentication logic of the app.
    /// </summary>
    public static class AuthenticationManager
    {
        /// <summary>
        /// Attempt to login user. If user doesn't exist return false. If user's credentials are wrong return false.
        /// </summary>
        /// <param name="userName">user name.</param>
        /// <param name="password">password.</param>
        /// <returns></returns>
        public static bool Authenticate(string userName, string password)
        {
            var user = App.UserRepository.GetAll().FirstOrDefault(x => x.UserName == userName);
            if (user == null)
            {
                return false;
            }

            if (userName == user.UserName && password == user.Password)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempt to register user in SQLite database. Return false if user already exists.
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        public static bool Register(string userName, string password)
        {
            var user = App.UserRepository.GetAll().FirstOrDefault(x => x.UserName == userName);
            if (user != null)
            {
                return false;
            }
            var userModel = new UserModel
            {
                UserName = userName,
                ImageContent = new byte[0],
                Password = password
            };
            App.UserRepository.Save(userModel);
            return true;
        }
    }
}