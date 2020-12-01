using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Models
{
    public class BiographicDynamic
    {
        public int BiographicId { get; set; }
        public string Name { get; set; }
        public string Answer { get; set; }
        public bool IsAnswered { get; set; }
        public List<BiographicDetail> BiographicDetails { get; set; }
    }

    public class BiographicDetail
    {
        public int Value { get; set; }
        public string Label { get; set; }
    }
}
