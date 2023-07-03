using IdentityExample.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityExample
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContext<AuthAppDbContext>(config =>
			{
				config.UseInMemoryDatabase("IdentityExample");
			});

			// Add bridge between database and Identity

			// F12 on AddIdentity, AddIdentity(TUser: class, TRole: class)
			// User == user object, s name, email etc
			// Register all the identity services
			builder.Services.AddIdentity<IdentityUser, IdentityRole>(config =>
			{
				config.Password.RequiredLength = 4;
				config.Password.RequireDigit = false;
				config.Password.RequireNonAlphanumeric = false;
				config.Password.RequireUppercase = false;
			}).AddEntityFrameworkStores<AuthAppDbContext>()
			  .AddDefaultTokenProviders();
			// add default tokens to url for standard operations like password reset

			builder.Services.ConfigureApplicationCookie(config =>
			{
				config.Cookie.Name = "Identity.Cookie";
				config.LoginPath = "/Home/Login";
			});

			builder.Services.AddControllersWithViews();


			var app = builder.Build();


			app.UseRouting();

			// Who are you?
			app.UseAuthentication();

			// Are you allowed? 
			app.UseAuthorization();


#pragma warning disable ASP0014
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
			});
#pragma warning restore ASP0014
			app.Run();
		}
	}
}