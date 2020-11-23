using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Entities
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public int SurveyId { get; set; }
        public int TenantId { get; set; }
        public int ClientId { get; set; }
        public string Reference { get; set; }
        public DateTime DateStamp { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime FirstReminder { get; set; }
        public DateTime SecondReminder { get; set; }
        public DateTime Escalate { get; set; }
        public string EscalateEmail { get; set; }
        public bool IsProjectStarted { get; set; }

    }
}
