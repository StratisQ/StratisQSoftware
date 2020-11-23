using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Entities
{
    public class KickOffProject
    {
        public int KickOffProjectId { get; set; }
        public int ProjectId { get; set; }
        public string Reference { get; set; }
        public DateTime DateStamp { get; set; }
    }

    public class ResendKickOffProject
    {
        public int KickOffProjectId { get; set; }
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
        public string Reference { get; set; }
        public DateTime DateStamp { get; set; }
    }
}
