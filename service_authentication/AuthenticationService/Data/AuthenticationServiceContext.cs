using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DomainModel;

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
