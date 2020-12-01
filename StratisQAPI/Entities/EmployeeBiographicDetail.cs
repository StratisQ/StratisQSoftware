using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Entities
{
    public class EmployeeBiographicDetail
    {
        public int EmployeeBiographicDetailId { get; set; }
        public int EmployeeId { get; set; }
        public int BiographicId { get; set; }
        public int BiographicDetailId { get; set; }
        public int ProjectId { get; set; }
        public DateTime DateStamp { get; set; }
    }
}
