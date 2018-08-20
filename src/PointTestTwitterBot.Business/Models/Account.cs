namespace PointTestTwitterBot.Business.Models
{
    public class Account
    {
        public Account(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; protected set; }
        public string Password { get; protected set; }
    }
}
