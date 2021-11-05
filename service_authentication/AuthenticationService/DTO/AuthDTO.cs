namespace AuthenticationService.DTO
{
    public class AuthDTO
    {
        private string _userName;
        private string _password;

        public string UserName { get => _userName; set => _userName = value; }
        public string Password { get => _password; set => _password = value; }

        public AuthDTO()
        {

        }

        public AuthDTO(string userName, string password)
        {
            _userName = userName;
            _password = password;
        }
    }
}
