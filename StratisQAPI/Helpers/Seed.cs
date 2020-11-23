using Microsoft.AspNetCore.Identity;
using StratisQAPI.Data;
using StratisQAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratisQAPI.Helpers
{
    public class Seed
    {
        private readonly StratisQDbContextUsers _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public Seed(StratisQDbContextUsers context,  UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async void SeedRoles()
        {
            string[] roles = new string[] { "STRATISQ", "ADMINISTRATOR", "USER", "GUEST" };

            foreach (string role in roles)
            {
                var r = _context.Roles.Where(u => u.Name == role).Any();

                if (r == false)
                {
                    _context.Roles.Add(new IdentityRole()
                    {
                        Name = role,
                        NormalizedName = role.ToUpper()
                    });
                    _context.SaveChanges();
                }
            }
        }

        public void SaveTenant()
        {
            Tenant tenant = new Tenant()
            {
                TenantName = "Default",
                TenantKey = "DEFAULT",
                DateStamp = DateTime.Now,
                Reference = "Admin"
            };

            Tenant tenantChecker = _context.Tenants.FirstOrDefault(id => id.TenantKey == "DEFAULT");

            if (tenantChecker == null)
            {
                _context.Tenants.Add(tenant);
                _context.SaveChanges();
            }
        }

        public void SeedUser()
        {
            var user = _context.Users.Where(u => u.Email == "web@stratisq.com").Any();

            if (user == false)
            {

                ApplicationUser userObj = new ApplicationUser
                {
                    Email = "web@stratisq.com",
                    UserName = "web@stratisq.com",
                    FirstName = "System Account",
                    LastName = "System Account",
                    DateStamp = DateTime.Now,
                    TenantId = 1 //Default Tenant
                };

                var p = "StratisQ@001";
                IdentityResult result = _userManager.CreateAsync(userObj, p).Result;

                _context.SaveChanges();


                //Assign default user to role StratisQ
                AssignRoleToUser(userObj, "STRATISQ");
            }

        }

        public async void AssignRoleToUser(ApplicationUser user, string role)
        {
            _userManager.AddToRoleAsync(user, "STRATISQ");
            _context.SaveChanges();
        }

    }
}
