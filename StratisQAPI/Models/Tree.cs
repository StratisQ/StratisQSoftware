using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Models
{
    public class Tree
    {
        public string Name { get; set; }
        public int NodeId { get; set; }
        public int ParentNodeId { get; set; }
        public int NodeType { get; set; }
        public List<Tree> Children { get; set; }
        public bool Checked { get; set; }
        public int Size { get; set; }
    }
}
