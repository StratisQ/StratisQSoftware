using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Models
{
    public class OrgStructure
    {
        public int NodeId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public List<OrgStructure> Subordinates { get; set; }

    }
}
