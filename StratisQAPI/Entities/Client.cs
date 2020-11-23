using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Entities
{
    public class Client
    {
        public int ClientId { get; set; }
        public int IndustryId { get; set; }
        [ForeignKey("TenantId")]
        public Industry Industry { get; set; }
        public string Reference { get; set; }
        public DateTime DateStamp { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Email { get; set; }
        public string RegistrationNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public int TenantId { get; set; }
        //[ForeignKey("TenantId")]
        //public Tenant Tenant { get; set; }
        public string VatNumber { get; set; }
        public string Website { get; set; }
    }
}
