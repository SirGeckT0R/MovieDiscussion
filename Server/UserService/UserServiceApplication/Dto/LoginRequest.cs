namespace UserServiceApplication.Dto
{
    public record LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public LoginRequest() { }
        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
