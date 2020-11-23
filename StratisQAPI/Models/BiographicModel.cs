using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Models
{
    public class BiographicModel
    {
        public string Name { get; set; }
        public int TenantId { get; set; }
        public int ClientId { get; set; }
        public string Reference { get; set; }
    }

    public class BiographicDetailModel
    {
        public string Name { get; set; }
        public int BiographicId { get; set; }
        public int TenantId { get; set; }
        public int ClientId { get; set; }
        public string Reference { get; set; }
    }
}
