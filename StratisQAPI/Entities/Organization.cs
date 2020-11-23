using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Entities
{
    public class Organization
    {
        public int OrganizationId { get; set; }
        public int ParentOrganizationId { get; set; }
        public int RootOrganizationId { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime DateStamp { get; set; }
        public string Reference { get; set; }
        public DateTime LastEditedDate { get; set; }
        public string LastEditedBy { get; set; }
        public int ClientId { get; set; }
        public int TenantId { get; set; }
    }
}
