using CPI.API.Auth.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace CPI.API.Configurations;

public static class AuthConfig
{
    public static IServiceCollection AddAuthConfiguration(this IServiceCollection  services, IConfiguration configuration)
    {
        var tokenConfiguration = configuration.GetSection("TokenConfiguration").Get<TokenConfiguration>();
        configuration.GetSection("TokenConfigurations").Bind(tokenConfiguration);

        services.AddSingleton(tokenConfiguration);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = tokenConfiguration.Issuer,
                ValidAudience = tokenConfiguration.Audience,
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenConfiguration.Secret))
            };
        });

        services.AddAuthorization(auth =>
        {
            auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());
        });

        return services;
    }
}
