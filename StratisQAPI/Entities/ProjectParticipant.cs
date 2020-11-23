using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Entities
{
    public class ProjectParticipant
    {
        public int ProjectParticipantId { get; set; }
        public int ProjectId { get; set; }
        public int ParticipantId { get; set; }
    }
}
