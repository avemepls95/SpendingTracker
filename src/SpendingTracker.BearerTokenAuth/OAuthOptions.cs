using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SpendingTracker.BearerTokenAuth
{
    public sealed class OAuthOptions
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public int AccessTokenLifetimeInMinutes { get; set; }
        public int SessionTimeInHours { get; set; }

        public OAuthOptions()
        {
        }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        }
    }
}