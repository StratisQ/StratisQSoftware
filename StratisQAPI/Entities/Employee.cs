using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public int TenantId { get; set; }
        public int ClientId { get; set; }
        public DateTime DateStamp { get; set; }
        public string Reference { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int AssetNodeId { get; set; }
        public bool IsSendEmail { get; set; }
        public Guid Identifier { get; set; }
    }
}
