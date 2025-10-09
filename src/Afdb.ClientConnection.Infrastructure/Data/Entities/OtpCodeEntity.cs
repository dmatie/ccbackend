using System;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities
{
    public class OtpCodeEntity : BaseEntityConfiguration
    {
        public string Email { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}