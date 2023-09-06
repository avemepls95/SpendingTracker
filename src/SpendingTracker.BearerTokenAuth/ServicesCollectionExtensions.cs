﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SpendingTracker.BearerTokenAuth.Abstractions;

namespace SpendingTracker.BearerTokenAuth
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddJwtBearerTokenAuth(
            this IServiceCollection servicesCollection,
            OAuthOptions authOptions)
        {
            servicesCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issuer,

                        ValidateAudience = false,

                        ValidateLifetime = true,
                        ClockSkew = new TimeSpan(0),
                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });

            servicesCollection.AddSingleton(authOptions);

            servicesCollection.AddSingleton<ITokenGenerator, TokenGenerator>();

            return servicesCollection;
        }
    }
}