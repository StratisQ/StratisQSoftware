using StratisQAPI.Data;
using StratisQAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Services
{
    public class OrganizationService
    {
        private readonly StratisQDbContext _context;
        private readonly StratisQDbContextUsers _dbContextUsers;

        public OrganizationService(StratisQDbContext context, StratisQDbContextUsers dbContextUsers)
        {
            _context = context;
            _dbContextUsers = dbContextUsers;
        }
        public List<OrganizationVM> GetOrganizationVMs(OrganisationalStructureLookup reference)
        {

            var orginisations = _context.Organizations.Where(a => (a.TenantId == reference.TenantId) && (a.ClientId == reference.ClientId)).OrderBy(a => a.Height).ToList();

            List<OrganizationVM> orginisationsVM = new List<OrganizationVM>();

            foreach (var item in orginisations)
            {
                orginisationsVM.Add(new OrganizationVM()
                {
                    //Code = item.Code,
                    RootOrganizationId = item.RootOrganizationId,
                    DateStamp = item.DateStamp,
                    Name = item.Name,
                    OrganizationId = item.OrganizationId,
                    ParentOrganizationId = item.ParentOrganizationId,
                    Reference = item.Reference,
                    Height = item.Height,
                    NodeId = item.OrganizationId,
                    TenantId = item.TenantId
                    //NodeType = 1 /*Type = 1 if organization*/
                });
            }

            return orginisationsVM;
        }
    }
}
