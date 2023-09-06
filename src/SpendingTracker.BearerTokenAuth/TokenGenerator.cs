﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using SpendingTracker.BearerTokenAuth.Abstractions;

namespace SpendingTracker.BearerTokenAuth
{
    internal class TokenGenerator : ITokenGenerator
    {
        private readonly OAuthOptions _authOptions;
        private readonly IMemoryCache _cachingService;

        public TokenGenerator(OAuthOptions authOptions, IMemoryCache cachingService)
        {
            _authOptions = authOptions;
            _cachingService = cachingService;
        }

        public TokenInformation Create<T>(T id) where T: struct
        {
            var claims = GetClaims(id);

            var credentials = new SigningCredentials(
                _authOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256);

            var expiresIn = DateTimeOffset.Now.AddMinutes(_authOptions.AccessTokenLifetimeInMinutes);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresIn.DateTime,
                SigningCredentials = credentials,
                Issuer = _authOptions.Issuer
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = GenerateRefreshToken();

            _cachingService.Set(id.ToString(), refreshToken);

            var result = new TokenInformation
            {
                AccessToken = tokenHandler.WriteToken(accessToken),
                ExpiresIn = expiresIn,
                RefreshToken = refreshToken
            };

            return result;
        }

        public TokenInformation Refresh<T>(string refreshToken, T id) where T: struct
        {
            EnsureRefreshTokenIsValidAsync(id.ToString()!, refreshToken);

            var newTokenInformation = Create(id);
            return newTokenInformation;
        }

        private void EnsureRefreshTokenIsValidAsync(string idAsString, string refreshToken)
        {
            var storedRefreshToken = _cachingService.Get<string>(idAsString);

            if (storedRefreshToken is null || !storedRefreshToken.Equals(refreshToken, StringComparison.Ordinal))
            {
                throw new UnauthorizedAccessException("Указанный refresh token не найден.");
            }
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
        
        private Claim[] GetClaims<T>(T id) where T: struct
        {
            var claims = new Claim[]
            {
                new("Id", id.ToString())
            };

            return claims;
        }
    }
}