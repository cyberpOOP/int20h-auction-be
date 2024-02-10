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

namespace Auction.WebAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuctionContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.Configure<JwtOptionsHelper>(configuration.GetSection("Jwt"));
        services.AddScoped<IMigrationHelper, MigrationHelper>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IBidService, BidService>();
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
        });
        services.AddAuthorization();
    }
}
