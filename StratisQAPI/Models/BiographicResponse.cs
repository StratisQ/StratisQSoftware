using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Models
{
    public class BiographicResponse
    {
        public int BiographicId { get; set; }
        public int BiographicDetailId { get; set; }
        public string EmployeeIdentifier { get; set; }
        public string ProjectIdentifier { get; set; }
    }
}
