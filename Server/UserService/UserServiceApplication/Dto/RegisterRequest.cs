namespace UserServiceApplication.Dto
{
    public record RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }

        public RegisterRequest(string email, string password, string username)
        {
            Email = email;
            Password = password;
            Username = username;
        }
    }
}
