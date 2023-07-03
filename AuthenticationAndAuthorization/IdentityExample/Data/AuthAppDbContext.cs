using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityExample.Data
{
	// IdentityDbContext contains all the user tables 
	public class AuthAppDbContext: IdentityDbContext
	{
		public AuthAppDbContext(DbContextOptions<AuthAppDbContext> options)
			: base(options) { }
	}
}
