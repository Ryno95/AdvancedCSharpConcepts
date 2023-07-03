using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Basics.Controllers.HomeController
{
	public class HomeController: Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		[Authorize]
		public IActionResult Secure()
		{
			return View();
		}


		public IActionResult Authenticate()
		{

			// This block usually happens after login credentials have been approved
			// The claims get filled from the database, for example the identity db using microsoft Identity
			var grandmaClaims = new List<Claim>()
			{
				new Claim(ClaimTypes.Name, "Bob"),
				new Claim(ClaimTypes.Email, "Bob@fmail.com"),
				new Claim("Custom claim type", "Good boy")
			};

			var licenseClaims = new List<Claim>()
			{
				new Claim(ClaimTypes.Name, "Bob K Foo"),
				new Claim("Driving license", "A+")
			};

			// Can have many identities
			var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Indentity");
			var lisenceIdentiry = new ClaimsIdentity(licenseClaims, "Government Identity");

			// Array of identities
			var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, lisenceIdentiry });


			// This gets called under the hood
			// Base implementation
			HttpContext.SignInAsync(userPrincipal);

			return RedirectToAction("Index");
		}
	}
}
