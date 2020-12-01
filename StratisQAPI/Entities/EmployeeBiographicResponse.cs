using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Entities
{
    public class EmployeeBiographicResponse
    {
        public int EmployeeBiographicResponseId { get; set; }
        public int BiographicId { get; set; }
        public string BiographicName { get; set; }
        public int BiographicDetailId { get; set; }
        public string BiographicDetailName { get; set; }
        public int EmployeeId { get; set; }
        public DateTime DateStamp { get; set; }
        public string Reference { get; set; }
    }
}
