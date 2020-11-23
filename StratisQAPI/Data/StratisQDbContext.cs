using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StratisQAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Data
{
    public class StratisQDbContext : DbContext
    {
        public StratisQDbContext(DbContextOptions<StratisQDbContext> options) : base(options)
        {

        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<AssetNode> AssetNodes { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ClientEmployee> ClientEmployees { get; set; }
        public DbSet<Biographic> Biographics { get; set; }
        public DbSet<BiographicDetail> BiographicDetails { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectParticipant> ProjectParticipants { get; set; }
        public DbSet<RemoveProjectParticipant> RemoveProjectParticipants { get; set; }
        public DbSet<KickOffProject> KickOffProjects { get; set; }
    }
}
