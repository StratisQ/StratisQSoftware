using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StratisQAPI.Data;
using StratisQAPI.Entities;
using StratisQAPI.Models;

namespace StratisQAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly StratisQDbContextUsers _contextUsers;
        private readonly StratisQDbContext _context;
        public EmployeeController(StratisQDbContext context, StratisQDbContextUsers contextUsers)
        {
            _context = context;
            _contextUsers = contextUsers;
        }
        

        [HttpPost("employeeListKickOff")]
        public IActionResult GetClientEmployeesForKickOff([FromBody] OrganisationalStructureLookup model)
        {

            if (model == null)
            {
                return BadRequest("Make sure the form is filled correctly.");
            }
            try
            {
                List<ClientEmployee> clientEmployees = _context.ClientEmployees.Where(id => (id.ClientId == model.ClientId) && (id.TenantId == model.TenantId)).ToList();
                List<Employee> lEmployees = new List<Employee>();

                foreach (var item in clientEmployees)
                {
                    lEmployees.Add(_context.Employees.FirstOrDefault(id => (id.EmployeeId == item.EmployeeId)));
                }

                var employees = lEmployees.Select(result => new
                {
                    EmployeeId = result.EmployeeId,
                    Name = result.Name,
                    Surname = result.Surname,
                    Email = result.Email,
                    Title = result.Title,
                    IsParticipant = GetParticipantStatus(result.EmployeeId, model.ProjectId),
                    //Root = _context.AssetNodes.FirstOrDefault(id => id.RootAssetNodeId == _context.AssetNodes.FirstOrDefault(id => id.AssetNodeId == result.AssetNodeId).RootAssetNodeId).Name,
                    Organization = GetOrganizationName(result.AssetNodeId),
                    Reeference = _contextUsers.Users.FirstOrDefault(id => id.Id == result.Reference).FirstName
                });

                return Ok(employees);
            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened! " + Ex.Message);
            }
        }

        public bool GetParticipantStatus(int employeeId, int projectId)
        {
            bool isParticipant = false;

            RemoveProjectParticipant removeProjectParticipant = _context.RemoveProjectParticipants.FirstOrDefault(id => (id.ProjectId == projectId) && (id.EmployeeId == employeeId));

            if (removeProjectParticipant == null)
            {
                isParticipant = true;
            }

            return isParticipant;
        }

        public string GetOrganizationName(int assetNodeId)
        {
            string organizationName = "";

            if (assetNodeId == 0)
            {
                return organizationName;
            }

            organizationName = _context.AssetNodes.FirstOrDefault(id => id.AssetNodeId == assetNodeId).Name;

            return organizationName;
        }

        [HttpPost("employeeList")]
        public IActionResult GetClientEmployees([FromBody] OrganisationalStructureLookup model)
        {

            if (model == null)
            {
                return BadRequest("Make sure the form is filled correctly.");
            }
            try
            {
                List<ClientEmployee> clientEmployees = _context.ClientEmployees.Where(id => (id.ClientId == model.ClientId) && (id.TenantId == model.TenantId)).ToList();
                List<Employee> employees = new List<Employee>();

                foreach (var item in clientEmployees)
                {
                    employees.Add(_context.Employees.FirstOrDefault(id => (id.EmployeeId == item.EmployeeId)));
                }

                return Ok(employees);
            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened. " + Ex.Message);
            }
        }


        [HttpPost("")]
        public IActionResult SaveEmployee([FromBody] Employee model)
        {
            if (model == null)
            {
                return BadRequest("Error, Make sure form is complete!");
            }

            try
            { 
                Employee employee = new Employee();
                Employee isEmployee = _context.Employees.FirstOrDefault(id => (id.Email == model.Email) && (id.TenantId == model.TenantId) && (id.ClientId == model.ClientId));
                if (isEmployee == null)
                {
                    employee.DateStamp = DateTime.Now;
                    employee.Email = model.Email;
                    employee.Name = model.Name;
                    employee.Surname = model.Surname;
                    employee.Reference = model.Reference;
                    employee.Title = model.Title;
                    employee.IsSendEmail = model.IsSendEmail;
                    employee.ClientId = model.ClientId;
                    employee.TenantId = model.TenantId;
                    employee.AssetNodeId = model.AssetNodeId;
                    employee.Identifier = Guid.NewGuid();

                    _context.Employees.Add(employee);
                    _context.SaveChanges();
                }


                ClientEmployee isClientEmployee = _context.ClientEmployees.FirstOrDefault(id => (id.TenantId == model.TenantId) && (id.ClientId == model.ClientId) && (id.EmployeeId == model.EmployeeId));

                if (isClientEmployee == null)
                {
                    ClientEmployee clientEmployee = new ClientEmployee();
                    clientEmployee.DateStamp = DateTime.Now;
                    clientEmployee.ClientId = model.ClientId;
                    clientEmployee.TenantId = model.TenantId;
                    clientEmployee.EmployeeId = employee.EmployeeId;
                    clientEmployee.Reference = model.Reference;

                    _context.ClientEmployees.Add(clientEmployee);
                    _context.SaveChanges();
                }


                return Ok();
            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened! "+ Ex.Message);
            }
        }

    }
}
