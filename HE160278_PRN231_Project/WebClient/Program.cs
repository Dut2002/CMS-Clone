using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;
using System.Text;
using WebClient.Connection;
using WebClient.CustomHandler;

namespace WebClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
			// Add services to the container.

			builder.Services.AddHttpClient();
			builder.Services.AddScoped<ICallApi, CallApi>(); // Đăng ký CallApi là một dịch vụ scoped

            // Cấu hình dịch vụ HttpClient để sử dụng cookie
            builder.Services.AddHttpClient<ICallApi, CallApi>()
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer(),
            });


            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				 .AddCookie(config =>
				 {
					 config.Cookie.Name = "UserLoginCookie";
					 config.LoginPath = "/Identity/Account/Login";
					 config.AccessDeniedPath = "/Identity//Account/AccessDenied";
				 });

			builder.Services.AddAuthorization(config =>
			{
				config.AddPolicy("UserPolicy", policyBuilder =>
				{
					policyBuilder.UserRequireCustomClaim(ClaimTypes.Name);
				});
			});

			builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }




            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}