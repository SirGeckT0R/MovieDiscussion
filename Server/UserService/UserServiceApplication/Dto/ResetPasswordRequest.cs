
namespace UserServiceApplication.Dto
{
    public record ResetPasswordRequest
    {
        public string Email { get; init; } = string.Empty;
        public string Token { get; init; } = string.Empty;
        public string NewPassword { get; init; } = string.Empty;

        public ResetPasswordRequest() {}

        public ResetPasswordRequest(string email, string token, string newPassword)
        {
            Email = email;
            Token = token;
            NewPassword = newPassword;
        }
    }
}
