using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Entities
{
    public class EmployeeBiographic
    {
        public int EmployeeBiographicId { get; set; }
        public int EmployeeId { get; set; }
        public int BiographicId { get; set; }
        public int BiographicDetailId { get; set; }
        public int ClientId { get; set; }
        public DateTime DateStamp { get; set; }
        public string BiographicName { get; set; }
        public bool IsBiographicDetail { get; set; }

    }
}
