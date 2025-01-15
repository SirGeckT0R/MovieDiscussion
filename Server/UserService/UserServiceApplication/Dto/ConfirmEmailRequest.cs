namespace UserServiceApplication.Dto
{
    public record ConfirmEmailRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public ConfirmEmailRequest() { }
        public ConfirmEmailRequest(string email, string token)
        {
            Email = email;
            Token = token;
        }
    }
}
