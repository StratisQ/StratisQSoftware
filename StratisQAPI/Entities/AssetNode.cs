using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Entities
{
    public class AssetNode
    {
        public int AssetNodeId { get; set; }
        public int ParentAssetNodeId { get; set; }
        public int RootAssetNodeId { get; set; }
        public int ClientId { get; set; }
        public int TenantId { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public DateTime DateStamp { get; set; }
        public string Reference { get; set; }
        public DateTime LastEditedDate { get; set; }
        public string LastEditedBy { get; set; }

    }
}
