using StratisQAPI.Data;
using StratisQAPI.Entities;
using StratisQAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StratisQAPI.Services
{
    public class AssetNodeService
    {
        private readonly StratisQDbContext _context;
        private readonly StratisQDbContextUsers _contextUsers;

        public AssetNodeService(StratisQDbContext context, StratisQDbContextUsers contextUsers)
        {
            _context = context;
            _contextUsers = contextUsers;
        }
        public List<AssetNodeVM> GetAssetNodeVMs(OrganisationalStructureLookup reference)
        {

            List<AssetNode> assetNodes = new List<AssetNode>();

            assetNodes = _context.AssetNodes.Where(id => (id.TenantId == reference.TenantId) && (id.ClientId == reference.ClientId)).ToList();

            assetNodes.OrderBy(a => a.Height);

            List<AssetNodeVM> assetNodesVM = new List<AssetNodeVM>();

            foreach (var item in assetNodes)
            {
                assetNodesVM.Add(new AssetNodeVM()
                {
                    //Code = item.Code,
                    RootAssetNodeId = item.RootAssetNodeId,
                    DateStamp = item.DateStamp,
                    Name = item.Name,
                    AssetNodeId = item.AssetNodeId,
                    ParentAssetNodeId = item.ParentAssetNodeId,
                    Reference = item.Reference,
                    Height = item.Height,
                    NodeId = item.AssetNodeId,
                    ClientId = item.ClientId,
                    TenantId = item.TenantId,
                    Size = item.Size
                });
            }

            List<AssetNodeVM> assetNodesVMOrdered = assetNodesVM.OrderBy(id => id.AssetNodeId).ToList();
            return assetNodesVMOrdered;
        }
    }
}
