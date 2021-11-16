using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Data
{
    public class AuthenticationServiceContext : DbContext
    {
        public AuthenticationServiceContext (DbContextOptions<AuthenticationServiceContext> options)
            : base(options)
        {
        }
                
        public DbSet<DomainModel.Authentication> Authentication { get; set; }
        public DbSet<DomainModel.Medic> Medic { get; set; }
    }
}
