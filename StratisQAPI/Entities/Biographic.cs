using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Entities
{
    public class Biographic
    {
        public int BiographicId { get; set; }
        public string Name { get; set; }
        public int TenantId { get; set; }
        public int ClientId { get; set; }
        public string Reference { get; set; }
        public DateTime DateStamp { get; set; }
    }
}
