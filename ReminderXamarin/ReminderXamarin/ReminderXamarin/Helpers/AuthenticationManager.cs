namespace ReminderXamarin.Helpers
{
    /// <summary>
    /// Class for test purposes. Implements authentication logic of the app.
    /// </summary>
    public static class AuthenticationManager
    {
        private const string UserName = "Ars";
        private const string Password = "1";

        public static bool Authenticate(string userName, string password)
        {
            if (userName == UserName && password == Password)
            {
                return true;
            }
            return false;
        }
    }
}