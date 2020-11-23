using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StratisQAPI.Data;
using StratisQAPI.Entities;
using StratisQAPI.Models;
using StratisQAPI.Services;

namespace StratisQAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssetSetupController : ControllerBase
    {
        private readonly StratisQDbContextUsers _contextUsers;
        private readonly StratisQDbContext _context;
        public AssetSetupController(StratisQDbContext context, StratisQDbContextUsers contextUsers)
        {
            _context = context;
            _contextUsers = contextUsers;
        }

        [HttpGet("tree")]
        public IActionResult GetTree(OrganisationalStructureLookup reference)
        {

            List<Tree1> data = new List<Tree1>();
            Tree1 rootTree = null;

            List<OrganizationVM> organizations = new OrganizationService(_context, _contextUsers).GetOrganizationVMs(reference);

            foreach (var org in organizations.Where(a => a.Height == 1))
            {
                rootTree = new Tree1();
                rootTree.Name = org.Name;
                rootTree.Children = new List<Tree1>();
                rootTree.NodeId = org.OrganizationId;
                rootTree.ParentNodeId = org.ParentOrganizationId;
                //rootTree.NodeType = org.NodeType;
                rootTree.Checked = false;

                data.Add(rootTree);

                var allChildrens = organizations.Where(a => a.RootOrganizationId == org.OrganizationId).OrderBy(p => p.Height).OrderBy(p => p.ParentOrganizationId).ToList();

                if (allChildrens.Count > 0)
                {
                    var maxHeight = allChildrens.Max(a => a.Height);

                    Tree1 currntParent = rootTree;
                    List<Tree1> parents = new List<Tree1>();

                    for (int i = 2; i <= maxHeight; i++)
                    {
                        Tree1 treeBranch = null;
                        Tree1 currentParent = null;

                        var t = allChildrens.OrderBy(h => h.Height).ToList();

                        var parentIds = t.Where(a => a.Height == i).Select(a => a.ParentOrganizationId).Distinct().ToList();

                        foreach (var id in parentIds)
                        {
                            foreach (var child in t.Where(a => a.Height == i && a.ParentOrganizationId == id))
                            {
                                treeBranch = new Tree1();
                                treeBranch.NodeId = child.OrganizationId;
                                treeBranch.ParentNodeId = child.ParentOrganizationId;
                                treeBranch.Name = child.Name;
                                treeBranch.Children = new List<Tree1>();
                                treeBranch.Checked = false;

                                parents.Add(treeBranch);

                                if (child.ParentOrganizationId != rootTree.NodeId)
                                    currentParent = parents.FirstOrDefault(a => a.NodeId == child.ParentOrganizationId);

                                if (currentParent != null)
                                {
                                    currentParent.Children.Add(treeBranch);
                                }
                                else
                                    rootTree.Children.Add(treeBranch);
                            }
                            if (currentParent != null)
                            {
                                rootTree = currentParent;
                            }
                            else
                                rootTree = treeBranch;
                        }

                    }

                }

            }

            return Ok(data);
        }

        [HttpPost("assetList")]
        public IActionResult GetAssets(OrganisationalStructureLookup reference)
        {
            List<AssetNode> assetNodes = _context.AssetNodes.Where(id => (id.ClientId == reference.ClientId) && (id.TenantId == reference.TenantId)).ToList();

            return Ok(assetNodes);
        }

        [HttpPost("assetNodeTree")]
        public IActionResult GetAssetNodeTree(OrganisationalStructureLookup reference)
        {

            List<Tree1> data = new List<Tree1>();
            Tree1 rootTree = null;

            List<AssetNodeVM> assetNodes = new AssetNodeService(_context, _contextUsers).GetAssetNodeVMs(reference);

            AssetNodeVM assetNodeVM = assetNodes.FirstOrDefault(id => id.Height == 1);

            if (assetNodeVM == null)
            {
                if (assetNodes.Count > 0)
                {
                    int firstHeight = assetNodes[0].Height - 1;
                    List<AssetNodeVM> assetNodesTemp = new List<AssetNodeVM>();
                    int flyRoot = assetNodes[0].AssetNodeId;
                    foreach (var an in assetNodes)
                    {
                        AssetNodeVM assetNodeVM1 = new AssetNodeVM();
                        assetNodeVM1 = an;
                        assetNodeVM1.Height = assetNodeVM1.Height - firstHeight;
                        assetNodeVM1.RootAssetNodeId = flyRoot;
                        assetNodesTemp.Add(assetNodeVM1);
                    }

                    assetNodes = new List<AssetNodeVM>();
                    assetNodes = assetNodesTemp;
                }

            }



            foreach (var asd in assetNodes.Where(a => a.Height == 1))
            {
                rootTree = new Tree1();
                rootTree.Name = string.Format("{0} ({1})", asd.Name, asd.Size);
                rootTree.Children = new List<Tree1>();
                rootTree.NodeId = asd.AssetNodeId;
                rootTree.ParentNodeId = asd.ParentAssetNodeId;
                //rootTree.NodeType = asd.NodeType;
                rootTree.Checked = false;

                data.Add(rootTree);

                var allChildrens = assetNodes.Where(a => a.RootAssetNodeId == asd.AssetNodeId)
                    .OrderBy(p => p.Height).OrderBy(p => p.ParentAssetNodeId).ToList();

                if (allChildrens.Count > 0)
                {
                    var maxHeight = allChildrens.Max(a => a.Height);

                    Tree1 currntParent = rootTree;
                    List<Tree1> parents = new List<Tree1>();

                    for (int i = 2; i <= maxHeight; i++)
                    {
                        Tree1 treeBranch = null;
                        Tree1 currentParent = null;

                        var t = allChildrens.OrderBy(h => h.Height).ToList();

                        var parentIds = t.Where(a => a.Height == i).Select(a => a.ParentAssetNodeId).Distinct().ToList();

                        foreach (var id in parentIds)
                        {
                            foreach (var child in t.Where(a => a.Height == i && a.ParentAssetNodeId == id))
                            {
                                treeBranch = new Tree1();
                                treeBranch.NodeId = child.AssetNodeId;
                                treeBranch.ParentNodeId = child.ParentAssetNodeId;
                                //treeBranch.NodeType = child.NodeType;
                                treeBranch.Name = string.Format("{0} ({1})", child.Name, child.Size);
                                treeBranch.Children = new List<Tree1>();
                                treeBranch.Checked = false;

                                parents.Add(treeBranch);

                                if (child.ParentAssetNodeId != rootTree.NodeId)
                                    currentParent = parents.FirstOrDefault(a => a.NodeId == child.ParentAssetNodeId);

                                if (currentParent != null)
                                {
                                    currentParent.Children.Add(treeBranch);
                                }
                                else
                                    rootTree.Children.Add(treeBranch);
                            }
                            if (currentParent != null)
                            {
                                rootTree = currentParent;
                            }
                            else
                                rootTree = treeBranch;
                        }

                    }

                }

            }

            return Ok(data);
        }

        [HttpPost("orgStructure")]
        public IActionResult GetOrgStructure(OrganisationalStructureLookup reference)
        {

            List<OrgStructure> data = new List<OrgStructure>();
            OrgStructure rootTree = null;

            List<AssetNodeVM> assetNodes = new AssetNodeService(_context, _contextUsers).GetAssetNodeVMs(reference);

            AssetNodeVM assetNodeVM = assetNodes.FirstOrDefault(id => id.Height == 1);

            if (assetNodeVM == null)
            {
                if (assetNodes.Count > 0)
                {
                    int firstHeight = assetNodes[0].Height - 1;
                    List<AssetNodeVM> assetNodesTemp = new List<AssetNodeVM>();
                    int flyRoot = assetNodes[0].AssetNodeId;
                    foreach (var an in assetNodes)
                    {
                        AssetNodeVM assetNodeVM1 = new AssetNodeVM();
                        assetNodeVM1 = an;
                        assetNodeVM1.Height = assetNodeVM1.Height - firstHeight;
                        assetNodeVM1.RootAssetNodeId = flyRoot;
                        assetNodesTemp.Add(assetNodeVM1);
                    }

                    assetNodes = new List<AssetNodeVM>();
                    assetNodes = assetNodesTemp;
                }

            }



            foreach (var asd in assetNodes.Where(a => a.Height == 1))
            {
                rootTree = new OrgStructure();
                rootTree.Name = asd.Name;
                rootTree.NodeId = asd.AssetNodeId;
                rootTree.Subordinates = new List<OrgStructure>();
                rootTree.Designation = "("+asd.Size+")";


                data.Add(rootTree);

                var allChildrens = assetNodes.Where(a => a.RootAssetNodeId == asd.AssetNodeId)
                    .OrderBy(p => p.Height).OrderBy(p => p.ParentAssetNodeId).ToList();

                if (allChildrens.Count > 0)
                {
                    var maxHeight = allChildrens.Max(a => a.Height);

                    OrgStructure currntParent = rootTree;
                    List<OrgStructure> parents = new List<OrgStructure>();

                    for (int i = 2; i <= maxHeight; i++)
                    {
                        OrgStructure treeBranch = null;
                        OrgStructure currentParent = null;

                        var t = allChildrens.OrderBy(h => h.Height).ToList();

                        var parentIds = t.Where(a => a.Height == i).Select(a => a.ParentAssetNodeId).Distinct().ToList();

                        foreach (var id in parentIds)
                        {
                            foreach (var child in t.Where(a => a.Height == i && a.ParentAssetNodeId == id))
                            {
                                treeBranch = new OrgStructure();
                                treeBranch.Name = child.Name;
                                treeBranch.NodeId = child.NodeId;
                                treeBranch.Subordinates = new List<OrgStructure>();
                                treeBranch.Designation = "(" + child.Size + ")";

                                parents.Add(treeBranch);

                                if (child.ParentAssetNodeId != rootTree.NodeId)
                                    currentParent = parents.FirstOrDefault(a => a.NodeId == child.ParentAssetNodeId);

                                if (currentParent != null)
                                {
                                    currentParent.Subordinates.Add(treeBranch);
                                }
                                else
                                    rootTree.Subordinates.Add(treeBranch);
                            }
                            if (currentParent != null)
                            {
                                rootTree = currentParent;
                            }
                            else
                                rootTree = treeBranch;
                        }

                    }

                }

            }

            return Ok(data);
        }


        [HttpPost("")]
        public IActionResult PostOrganization([FromBody] AssetNode assetNode)
        {
            if (assetNode == null)
            {
                return BadRequest("Something bad happened. Object is null");
            }

            if (assetNode.ParentAssetNodeId != 0)
            {

                AssetNode asset = _context.AssetNodes.FirstOrDefault(id => (id.TenantId == assetNode.TenantId) && (id.ClientId == assetNode.ClientId) && (id.Name == assetNode.Name));

                if (asset != null)
                {
                    return BadRequest("There is already a node with this name: " + assetNode.Name);
                }
            }

            try
            {
                AssetNode model = new AssetNode();
                model.ParentAssetNodeId = assetNode.ParentAssetNodeId;
                model.Name = assetNode.Name.ToUpper();
                model.DateStamp = DateTime.Now;
                model.Reference = assetNode.Reference;
                model.ClientId = assetNode.ClientId;
                model.TenantId = assetNode.TenantId;
                model.Size = assetNode.Size;
                model.LastEditedDate = DateTime.Now;
                model.LastEditedBy = assetNode.Reference;

                if (model.ParentAssetNodeId == 0)
                {
                    model.RootAssetNodeId = 0;
                }
                else
                {
                    model.RootAssetNodeId = _context.AssetNodes.FirstOrDefault(id => id.AssetNodeId == model.ParentAssetNodeId).RootAssetNodeId;
                }


                _context.AssetNodes.Add(model);
                _context.SaveChanges();

                 if (model.ParentAssetNodeId == 0)
                {
                    model.RootAssetNodeId = model.AssetNodeId;
                    model.Height = model.Height + 1;
                }
                else
                {
                    //model.RootAssetNodeId = _context.AssetNodes.FirstOrDefault(id => id.ParentAssetNodeId == model.ParentAssetNodeId).RootAssetNodeId;
                    model.Height = _context.AssetNodes.FirstOrDefault(id => id.AssetNodeId == model.ParentAssetNodeId).Height + 1;
                }

                _context.AssetNodes.Update(model);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened! " + Ex.Message);
            }
        }


    }
}
