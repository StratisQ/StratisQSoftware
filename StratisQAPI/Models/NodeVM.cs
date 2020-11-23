using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Models
{
    public class NodeVM
    {
        public int AssetNodeId { get; set; }
        public int RootAssetNodeId { get; set; }
        public int ParentAssetNodeId { get; set; }
        public string Name { get; set; }
        public DateTime DateStamp { get; set; }
        public string Reference { get; set; }
        public int Height { get; set; }
        public int NodeId { get; set; }
        public int TenantId { get; set; }
    }
}
