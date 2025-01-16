using UserServiceDataAccess.Enums;

namespace UserServiceDataAccess.Models
{
    public class Token : IdModel
    {
        public ETokenType TokenType { get; private set; } = ETokenType.None;
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;
        public string TokenValue { get; private set; } = string.Empty;
        public DateTime ExpiresAt { get; private set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public Token() { }

        public Token(Guid id, ETokenType tokenType, Guid userId, string tokenValue, DateTime expiresAt)
        {
            Id = id;
            TokenType = tokenType;
            UserId = userId;
            TokenValue = tokenValue;
            ExpiresAt = expiresAt;
        }
    }
}
