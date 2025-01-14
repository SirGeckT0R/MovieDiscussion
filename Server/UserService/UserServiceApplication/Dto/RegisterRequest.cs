namespace UserServiceApplication.Dto
{
    public record RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public RegisterRequest() { }

        public RegisterRequest(string email, string password, string username)
        {
            Email = email;
            Password = password;
            Username = username;
        }
    }
}
