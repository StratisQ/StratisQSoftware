using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Entities
{
    public class Tenant
    {
        public int TenantId { get; set; }
        [Required]
        [MaxLength(50)]
        public string TenantKey { get; set; }
        [Required]
        [MaxLength(50)]
        public string TenantName { get; set; }
        public string Reference { get; set; }
        public DateTime DateStamp { get; set; }
    }
}
