﻿using UserServiceDataAccess.Enums;

namespace UserServiceDataAccess.Models
{
    public class Token : IdModel
    {
        public TokenType TokenType { get; private set; } = TokenType.None;
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;
        public string TokenValue { get; private set; } = string.Empty;
        public DateTime ExpiresAt { get; private set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public Token() { }

        public Token(Guid id, TokenType tokenType, Guid userId, string tokenValue, DateTime expiresAt)
        {
            Id = id;
            TokenType = tokenType;
            UserId = userId;
            TokenValue = tokenValue;
            ExpiresAt = expiresAt;
        }
    }
}
