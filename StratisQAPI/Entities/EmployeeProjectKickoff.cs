using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Entities
{
    public class EmployeeProjectKickoff
    {
        public int EmployeeProjectKickoffId { get; set; }
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public bool IsKickedOff { get; set; }
        public DateTime DateStamp { get; set; }
        public string Reference { get; set; }
    }
}
