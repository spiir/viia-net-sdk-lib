using System;

namespace Viia.Core.Tokens
{
    public class Token
    {
        public string AccessToken { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset RefreshExpiresAt { get; set; }
        public string RefreshToken { get; set; }
    }
}
