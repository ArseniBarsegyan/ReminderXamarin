﻿using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ReminderXamarin;
using Rm.Data.Data.Entities;

namespace Rm.Helpers
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
            var user = App.UserRepository.Value.GetAll().FirstOrDefault(x => x.UserName == userName);
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
            var user = App.UserRepository.Value.GetAll().FirstOrDefault(x => x.UserName == userName);
            if (user != null)
            {
                return false;
            }
            var passwordBytes = Encoding.Unicode.GetBytes(password);
            var passwordHash = SHA256.Create().ComputeHash(passwordBytes);

            var userModel = new UserModel
            {
                UserName = userName,
                ImageContent = new byte[0],
                Password = passwordHash
            };
            App.UserRepository.Value.Save(userModel);
            return true;
        }
    }
}