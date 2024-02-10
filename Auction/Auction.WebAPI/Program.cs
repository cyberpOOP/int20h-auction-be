using Auction.DAL.Interfaces;
using Auction.WebAPI.Extensions;
using Auction.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.RegisterCustomServices(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomAutoMapperProfiles();
builder.Services.AddFluentValidation();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(opt => opt
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
SeedDatabase();
app.Run();

void SeedDatabase()
{
	using (var scope = app.Services.CreateScope())
	{
		var migrationHelper = scope.ServiceProvider.GetRequiredService<IMigrationHelper>();
		migrationHelper.Migrate();
	}
}