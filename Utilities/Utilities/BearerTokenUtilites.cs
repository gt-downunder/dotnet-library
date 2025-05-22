using System;
using System.IdentityModel.Tokens.Jwt;

namespace DotNet.Library.Utilities
{
    public static class BearerTokenUtilites
    {
        public static DateTimeOffset GetJwtExpiration(string bearerToken)
        {
            return new DateTimeOffset(new JwtSecurityToken(bearerToken).ValidTo);
        }
    }
}
