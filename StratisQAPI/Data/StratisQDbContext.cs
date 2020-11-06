using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StratisQAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Data
{
    public class StratisQDbContext : IdentityDbContext<ApplicationUser>
    {
        public StratisQDbContext(DbContextOptions<StratisQDbContext> options) : base(options)
        {

        }

        public DbSet<Country> Countries { get; set; }

    }
}
