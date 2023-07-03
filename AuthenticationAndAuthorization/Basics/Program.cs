namespace Basics
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddAuthentication("CookieAuth")
				.AddCookie("CookieAuth", config =>
				{
					// Set cookie upon being authenticated
					config.Cookie.Name = "Grandmas.Cookie";

					// overwrite default login path Account/Login?ReturnUrl=%2FHome%2FSecure
					config.LoginPath = "/Home/Authenticate";
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