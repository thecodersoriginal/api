using APICore.Helpers.Misc;
using APICore.Model.Auth;
using TaskTop.Authorization.Model;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace TaskTop.Authorization
{
    public class TokenConfig
    {
        public string Type { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Hours { get; set; }
    }

    public class SigningConfig
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfig()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }

    public static class AuthExts
    {
        public static Operator ToOperador(List<Claim> claims)
        {
            var userType = claims.GetClaim<string>(ClaimTypes.Role);
            var tipo = userType == null
                ? default(UserType)
                : Enum.Parse(typeof(UserType), userType).ChangeType<UserType>();

            return new Operator
            {
                Id = claims.GetClaim<int>(ClaimTypes.NameIdentifier),
                Type = tipo,
                Email = claims.GetClaim<string>(ClaimTypes.Email),
                Name = claims.GetClaim<string>(ClaimTypes.Name)
            };
        }

        public static Operator ToOperador(IIdentity identity) =>
            ToOperador(((ClaimsIdentity)identity).Claims.ToList());

        public static List<Claim> ToClaims(Operator user) => new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Type.ToString())
        };
    }
}
