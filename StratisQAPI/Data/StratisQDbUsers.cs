using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StratisQAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Data
{
    public class StratisQDbContextUsers: IdentityDbContext<ApplicationUser>
    {
        public StratisQDbContextUsers(DbContextOptions<StratisQDbContextUsers> options) : base(options)
        {

        }

        public DbSet<Tenant> Tenants { get; set; }

    }
}
