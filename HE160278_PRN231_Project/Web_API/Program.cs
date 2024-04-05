using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.Extensions.Configuration;
using Web_API.Models;
using Web_API.Models.Dto;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting.Internal;

namespace Web_API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllers();

			// Load configuration from appsettings.json
			builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

			var configuration = builder.Configuration;

/*			builder.Services.AddSingleton<IWebHostEnvironment>(provider => provider.GetRequiredService<IWebHostEnvironment>());
			builder.Host.UseContentRoot(Directory.GetCurrentDirectory());*/


			// Add database context
			builder.Services.AddDbContext<ApplicationDBContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("SqlConnection")));

			// Note: JWT Authentication and CustomAuthenticationMiddleware related configuration has been removed.

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAnyOrigin", builder =>
				{
					builder.AllowAnyOrigin()
						   .AllowAnyMethod()
						   .AllowAnyHeader();
				});
			});

			builder.Services.AddAuthorization();

			// Configure Swagger
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			{
				options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey
				});

				options.OperationFilter<SecurityRequirementsOperationFilter>();
			});

			var app = builder.Build();

			// Configure middleware
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var context = services.GetRequiredService<ApplicationDBContext>();
					context.Database.EnsureCreated();
					DbInitializer.Initialize(context);
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred creating the DB.");
				}
			}

			app.UseHttpsRedirection();

			// Removed app.UseMiddleware<CustomAuthenticationMiddleware>();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseCors("AllowAnyOrigin");


			app.MapControllers();

			app.Run();
		}
	}
}
