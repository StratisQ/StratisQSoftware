using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StratisQAPI.Data;
using StratisQAPI.Entities;
using StratisQAPI.Models;
using BiographicDetail = StratisQAPI.Models.BiographicDetail;
using Project = StratisQAPI.Entities.Project;

namespace StratisQAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly StratisQDbContext _context;
        private readonly StratisQDbContextUsers _contextUsers;

        public SurveyController(StratisQDbContext context, StratisQDbContextUsers contextUsers)
        {
            _context = context;
            _contextUsers = contextUsers;
        }

        [HttpGet("")]
        public IActionResult GetSurveyTemplates()
        {
            List<Survey> surveys = _context.Surveys.ToList();

            return Ok(surveys);
        }

        [HttpPost("respondBiographicDetail")]
        public IActionResult SaveBiographicResponse([FromBody] BiographicResponse biographicResponse)
        {
            if (biographicResponse == null)
            {
                return BadRequest("Error, Make sure your selection is valid");
            }

            try
            {
                Project project = _context.Projects.FirstOrDefault(id => id.ProjectIdentifier.ToString() == biographicResponse.ProjectIdentifier);
                Employee employee = _context.Employees.FirstOrDefault(id => (id.Identifier.ToString() == biographicResponse.EmployeeIdentifier));

                EmployeeBiographic response = _context.EmployeeBiographics.FirstOrDefault(id => (id.EmployeeId == employee.EmployeeId) && (id.BiographicId == biographicResponse.BiographicId));

                response.IsBiographicDetail = true;
                response.BiographicDetailId = biographicResponse.BiographicDetailId;

                _context.EmployeeBiographics.Update(response);
                _context.SaveChanges();

                return Ok(response);
            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened! " + Ex.Message);
            }

        }

        [HttpGet("biographicDetails")]
        public IActionResult GetParticipantBiographicDetails(string employeeIdentifier)
        {
            if (employeeIdentifier == "" || employeeIdentifier == null)
            {
                return BadRequest("Make sure you are allowed to participate in this survey.");
            }

            Client client = _context.Clients.FirstOrDefault(id => id.ClientId == _context.Employees.FirstOrDefault(id => id.Identifier.ToString() == employeeIdentifier).ClientId);
            Employee employee = _context.Employees.FirstOrDefault(id => id.Identifier.ToString() == employeeIdentifier.ToString());

            List<Biographic> biographics = _context.Biographics.Where(id => id.ClientId == client.ClientId).ToList();

            List<BiographicDynamic> biographicDynamics = new List<BiographicDynamic>();

            foreach (var b in biographics)
            {
                BiographicDynamic biographicDynamic = new BiographicDynamic();
                biographicDynamic.BiographicId = b.BiographicId;

                if (_context.EmployeeBiographics.FirstOrDefault(id=>(id.BiographicId == b.BiographicId) && (id.EmployeeId == employee.EmployeeId)).IsBiographicDetail)
                {
                    biographicDynamic.Answer = "["+_context.BiographicDetails.FirstOrDefault(id=>id.BiographicDetailId == _context.EmployeeBiographics.FirstOrDefault(id => (id.BiographicId == b.BiographicId) && (id.EmployeeId == employee.EmployeeId)).BiographicDetailId).Name+"]";
                }
                else
                {
                    biographicDynamic.Answer = "";
                }

                biographicDynamic.Name = b.Name;
                biographicDynamic.IsAnswered = _context.EmployeeBiographics.FirstOrDefault(id => (id.BiographicId == b.BiographicId) && (id.EmployeeId == employee.EmployeeId)).IsBiographicDetail;

                List<Entities.BiographicDetail> biographicDetails = _context.BiographicDetails.Where(id => id.BiographicId == b.BiographicId).ToList();
                List<BiographicDetail> biographicDetails1 = new List<BiographicDetail>();

                foreach (var item in biographicDetails)
                {
                    BiographicDetail dynamic = new BiographicDetail();
                    dynamic.Label = item.Name;
                    dynamic.Value = item.BiographicDetailId;
                    biographicDetails1.Add(dynamic);
                }

                biographicDynamic.BiographicDetails = biographicDetails1;

                biographicDynamics.Add(biographicDynamic);

            }

            return Ok(biographicDynamics);
        }

        [HttpPost("")]
        public IActionResult SaveSurveyTemplate([FromBody] Survey model)
        {
            if (model == null)
            {
                return BadRequest("Error, Make sure form is complete!");
            }

            try
            {
                Survey surveyChecker = _context.Surveys.FirstOrDefault(id => (id.Name.ToUpper() == model.Name.ToUpper()) && (id.Version == model.Version));

                if (surveyChecker != null)
                {
                    return BadRequest("Survey with this name and version already exists.");
                }

                Survey survey = new Survey();
                survey.Name = model.Name;
                survey.Version = model.Version;
                survey.RatingScale = model.RatingScale;

                _context.Surveys.Add(survey);
                _context.SaveChanges();

                return Ok(); 

            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened! " + Ex.Message);
            }

        }

        [HttpPost("employeeSurveyConsent")]
        public IActionResult GetEmployeeSurveyConsent([FromBody] EmployeeSurvey model)
        {
            if (model == null)
            {
                return BadRequest("Error, Contact your Manager/ Administrator!");
            }

            try
            {

                Employee employee = _context.Employees.FirstOrDefault(id => id.Identifier.ToString() == model.EmployeeIdentifier);
                Project project = _context.Projects.FirstOrDefault(id => id.ProjectIdentifier.ToString() == model.ProjectIdentifier);
                ProjectParticipant projectParticipant = _context.ProjectParticipants.FirstOrDefault(id => (id.ParticipantId == employee.EmployeeId) && (id.ProjectId == project.ProjectId));


                projectParticipant.IsAcknowledged = true;
                _context.ProjectParticipants.Update(projectParticipant);

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened! " + Ex.Message);
            } 
        }

        [HttpPost("employeeSurvey")]
        public IActionResult GetEmployeeSurvey([FromBody] EmployeeSurvey model)
        {
            if (model == null)
            {
                return BadRequest("Error, Make sure form is complete!");
            }

            try
            {

                Employee employee = _context.Employees.FirstOrDefault(id => id.Identifier.ToString() == model.EmployeeIdentifier);
                Project project = _context.Projects.FirstOrDefault(id => id.ProjectIdentifier.ToString() == model.ProjectIdentifier);
                ProjectParticipant projectParticipant = _context.ProjectParticipants.FirstOrDefault(id => (id.ParticipantId == employee.EmployeeId) && (id.ProjectId == project.ProjectId));

                if ((employee != null) && (project != null))
                {
                    var employeeSurvey = new
                    {
                        Name = employee.Name,
                        Surname = employee.Surname,
                        Email = employee.Email,
                        Organization = _context.AssetNodes.FirstOrDefault(id => id.AssetNodeId == employee.AssetNodeId).Name,
                        Root = _context.AssetNodes.FirstOrDefault(id => id.AssetNodeId == _context.AssetNodes.FirstOrDefault(id => id.AssetNodeId == employee.AssetNodeId).RootAssetNodeId).Name,
                        ProjectName = project.Name,
                        ProjectStartDate = project.StartDate.ToString("dd MMM yyyy"),
                        ProjectEndDate = project.EndDate.ToString("dd MMM yyyy"),
                        ProjectType = _context.Surveys.FirstOrDefault(id => id.SurveyId == project.SurveyId).Name,
                        IsParticipated = projectParticipant.IsParticipated,
                        IsAcknowleged = projectParticipant.IsAcknowledged
                    };

                    return Ok(employeeSurvey);
                }

                return Ok();

            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened. " + Ex.Message);
            }

        }

    }
}
