using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Entities
{
    public class Survey
    {
        public int SurveyId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public int RatingScale { get; set; }
        public bool Is360 { get; set; }
    }
}
