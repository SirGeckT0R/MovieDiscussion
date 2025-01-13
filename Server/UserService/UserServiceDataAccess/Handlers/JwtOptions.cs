
namespace UserServiceDataAccess.Handlers
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiresMinutesAccess { get; set; }
        public int ExpiresMinutesReset { get; set; }
        public int ExpiresHoursRefresh { get; set; }
        public int ExpiresHoursConfirm { get; set; }
    }
}
