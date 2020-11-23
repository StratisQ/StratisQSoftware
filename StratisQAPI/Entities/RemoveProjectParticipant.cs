using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Entities
{
    public class RemoveProjectParticipant
    {
        public int RemoveProjectParticipantId { get; set; }
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public DateTime DateStamp { get; set; }
        public string Reference { get; set; }
    }
}
