namespace PIM_IV.Entities
{
    public class Login
    {
        public string Username { get; }
        public string Password { get; }

        public Login(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
