using UserServiceDataAccess.Enums;

namespace UserServiceDataAccess.Models
{
    public class Token : IdModel
    {
        public E_TokenType TokenType { get; private set; } = E_TokenType.None;
        public User User { get; private set; } = null!;
        public string TokenValue { get; private set; } = string.Empty;
        public DateTime ExpiresAt { get; private set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public Token() { }

        public Token(Guid id, E_TokenType tokenType, User user, string tokenValue, DateTime expiresAt)
        {
            Id = id;
            TokenType = tokenType;
            User = user;
            TokenValue = tokenValue;
            ExpiresAt = expiresAt;
        }
    }
}
