using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ReminderXamarin.Data.Entities;

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
        public static async Task<bool> Authenticate(string userName, string password)
        {
            Console.WriteLine($"Warning: Warning '{DateTime.Now} BEFORE AUTHENTICATION !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!'");
            var user = (await App.UserRepository.GetAsync(x => x.UserName == userName)).FirstOrDefault();
            Console.WriteLine($"Warning: Warning '{DateTime.Now} AFTER AUTHENTICATION !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!'");
            if (user == null)
            {
                return false;
            }
            var passwordBytes = Encoding.Unicode.GetBytes(password);
            var passwordHash = SHA256.Create().ComputeHash(passwordBytes);

            if (userName == user.UserName && passwordHash.SequenceEqual(user.Password))
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
        public static async Task<bool> Register(string userName, string password)
        {
            var user = App.UserRepository.GetAll().FirstOrDefault(x => x.UserName == userName);
            if (user != null)
            {
                return false;
            }
            var passwordBytes = Encoding.Unicode.GetBytes(password);
            var passwordHash = SHA256.Create().ComputeHash(passwordBytes);

            var userModel = new AppUser
            {
                UserName = userName,
                ImageContent = new byte[0],
                Password = passwordHash
            };
            await App.UserRepository.CreateAsync(userModel);
            await App.UserRepository.SaveAsync();
            // App.UserRepository.Save(userModel);
            return true;
        }
    }
}