using cs_challenge.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace cs_challenge.Services
{
    public class TokenService : ITokenService
    {
        public string GenerateToken(string identifier, string securityKey) {
            var signingKey = new InMemorySymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var claimsIdentity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, identifier),
                new Claim(ClaimTypes.Role, "User"),
            }, "Custom");

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.CreateToken(securityTokenDescriptor);
            tokenHandler.WriteToken(jwtToken);

            return SecurityHelper.GenerateHash(jwtToken.ToString());
        }

        public bool ValidateToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}