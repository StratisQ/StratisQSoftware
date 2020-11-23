using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Entities
{
    public class ClientEmployee
    {
        public int ClientEmployeeId { get; set; }
        public int ClientId { get; set; }
        public int TenantId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime DateStamp { get; set; }
        public string Reference { get; set; }
    }
}
