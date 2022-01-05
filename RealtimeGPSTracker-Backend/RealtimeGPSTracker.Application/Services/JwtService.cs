using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealtimeGpsTracker.Core.Interfaces.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealtimeGpsTracker.Application.Services
{
    public class JwtService : IJwtService
    {
        public class JwtSettings
        {
            // Constructor needed for binding
            public JwtSettings() { }

            /// <summary>
            /// Secret key
            /// </summary>
            public string Key { get; set; } = null;

            /// <summary>
            /// (Issuer) Claim - The "iss" (issuer) claim identifies the principal that issued the JWT.
            /// </summary>
            public string Issuer { get; set; } = null;

            /// <summary>
            /// (Audience) Claim - The "aud" (audience) claim identifies the recipients that the JWT is intended for.
            /// </summary>
            public string Audience { get; set; } = null;

            /// <summary>
            /// Set the in value the token will be valid for (default is 120 min)
            /// </summary>
            public int MinutesValid { get; set; } = 0;
        }

        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            // Getting strongly typed Jwt Issuer, Audience, Key and MinutesValid settings
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateSecurityToken(string userId)
        {
            // Getting Jwt Key from jwt settings to create Issuer (Server) sign in key
            byte[] jwtSigningKeyBytes = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            SymmetricSecurityKey jwtIssuerSignInKey = new SymmetricSecurityKey(jwtSigningKeyBytes);
            SigningCredentials signingCredentials = new SigningCredentials(jwtIssuerSignInKey, SecurityAlgorithms.HmacSha512Signature);

            // Setting authentication claims
            var authClaims = new Claim[]
            {
                // User Id claim
                new Claim(JwtRegisteredClaimNames.Sub, userId)
            };

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: authClaims,
                notBefore: DateTime.UtcNow,
                //issued At
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.MinutesValid),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}