namespace SpendingTracker.BearerTokenAuth
{
    public class BasicAuthCredentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public BasicAuthCredentials()
        {
        }

        public BasicAuthCredentials(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public bool Equals(string userName, string password)
        {
            return string.Equals(UserName, userName, StringComparison.Ordinal)
                   && string.Equals(Password, password, StringComparison.Ordinal);
        }
    }
}