using System.Text;
using Auction.BLL.Interfaces;
using Auction.BLL.Mappers;
using Auction.BLL.Services;
using Auction.Common.Helpers;
using Auction.DAL.Context;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using FluentValidation.AspNetCore;
using FluentValidation;
using Auction.DAL.Interfaces;
using Auction.DAL.Helpers;
using System.Security.Claims;

namespace Auction.WebAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuctionContext>(options => options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]));
        services.Configure<JwtOptionsHelper>(configuration.GetSection("Jwt"));
        services.AddScoped<IMigrationHelper, MigrationHelper>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IBidService, BidService>();
        services.AddScoped<ICredentialService, CredentialService>();
    }

    public static void AddCustomAutoMapperProfiles(this IServiceCollection services)
    {
		services.AddAutoMapper(conf =>
		{
			conf.AddProfiles(
				new List<Profile>()
				{
						new DataMapperProfile(),
				});
		});
	}
	public static void AddFluentValidation(this IServiceCollection services)
	{
		services.AddFluentValidationAutoValidation();
		services.AddValidatorsFromAssemblyContaining(typeof(Program));
	}

	public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = config["Jwt:Issuer"],
                ValidAudience = config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
            };
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    var email = context?.Principal?.FindFirst("email");
                    if (email == null)
                    {
                        context?.Fail("NameClaimType is missing in the token.");
                    }
                    var serviceProvider = services.BuildServiceProvider();
                    var credentialService = serviceProvider.GetRequiredService<ICredentialService>();
                    if (! await credentialService.SetUser(email!.Value))
                    {
                        context?.Fail("No user found for provided email!");
                    }
                }
            };
        });
        services.AddAuthorization();
    }
}
