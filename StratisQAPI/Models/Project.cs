using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Models
{
    public class Project
    {
        public string Name { get; set; }
        public int SurveyId { get; set; }
        public int TenantId { get; set; }
        public int ClientId { get; set; }
        public string Reference { get; set; }
        public DateTime DateStamp { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime FirstReminder { get; set; }
        public TimeSpan FirstReminderTime { get; set; }
        public DateTime SecondReminder { get; set; }
        public TimeSpan SecondReminderTime { get; set; }
        public DateTime Escalate { get; set; }
        public TimeSpan EscalateTime { get; set; }
        public string EscalateEmail { get; set; }
    }
}
