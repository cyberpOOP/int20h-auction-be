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
using Azure.Storage.Blobs;

namespace Auction.WebAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuctionContext>(options => options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]));
        services.Configure<JwtOptionsHelper>(options =>
        {
			options.Audience = configuration["Jwt:Audience"];
			options.Issuer = configuration["Jwt:Issuer"];
			options.Key = configuration["Jwt:Key"];
			if (int.TryParse(configuration["Jwt:TokenExpiration"], out int tokenExpiration))
			{
				options.TokenExpiration = tokenExpiration;
			}
			else
			{
				options.TokenExpiration = 60;
			}
		});
        services.AddScoped<IMigrationHelper, MigrationHelper>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IBidService, BidService>();
        services.AddScoped<ICredentialService, CredentialService>();
        services.AddTransient<IAzureManagementService, AzureManagementService>();
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
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true
            };
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    var email = context?.Principal?.FindFirst(ClaimTypes.Email);
                    if (email == null || email.Value == null)
                    {
                        context?.Fail("NameClaimType is missing in the token.");
                        return;
                    }
                    var credentialService = context?.HttpContext.RequestServices.GetRequiredService<ICredentialService>();
                    if (! await credentialService.SetUser(email!.Value))
                    {
                        context?.Fail("No user found for provided email!");
                    }
                }
            };
        });
        services.AddAuthorization();
    }

    public static void RegisterAzureConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["AzureBlobStorageSettings:ConnectionString"];

        services.AddScoped(_ =>
                new BlobServiceClient(connectionString));

        services.AddSingleton(provider =>
        {
            var options = new BlobContainerOptionsHelper
            {
                BlobContainerName = configuration["AzureBlobStorageSettings:BlobContainerName"]
            };
            return options;
        });
    }
}
