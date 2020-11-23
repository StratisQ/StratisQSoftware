using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StratisQAPI.Data;
using StratisQAPI.Entities;

namespace StratisQAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly StratisQDbContext _context;
        private readonly StratisQDbContextUsers _contextUsers;
        private readonly IConfiguration _configuration;

        public ProjectController(StratisQDbContext context, StratisQDbContextUsers contextUsers, IConfiguration configuration)
        {
            _context = context;
            _contextUsers = contextUsers;
            _configuration = configuration;
        }

        [HttpGet("project")]
        public IActionResult GetProject(int projectId)
        {
            Project lProject = _context.Projects.FirstOrDefault(id => id.ProjectId == projectId);

            return Ok(lProject);
        }

        [HttpGet("")]
        public IActionResult GetProjects(int clientId)
        {
            List<Project> lProjects = _context.Projects.Where(id=>id.ClientId == clientId).ToList();

            var projects = lProjects.Select(result => new
            {
                ProjectId = result.ProjectId,
                Name = result.Name,
                Started = result.IsProjectStarted,
                Survey = _context.Surveys.FirstOrDefault(id=>id.SurveyId == result.SurveyId).Name,
                EndDate = result.EndDate.ToString("dd MMM yyyy"),
                StartDate = result.StartDate.ToString("dd MMM yyyy"),
                DateStamp = result.DateStamp,
                Reeference = _contextUsers.Users.FirstOrDefault(id => id.Id == result.Reference).FirstName
            });

            return Ok(projects);
        }

        [HttpPost("AddProjectParticipant")]
        public IActionResult AddProjParticipant([FromBody] RemoveProjectParticipant model)
        {
            if (model == null)
            {
                return BadRequest("Error, Make sure form is complete!");
            }

            try
            {
                RemoveProjectParticipant participantExists = _context.RemoveProjectParticipants.FirstOrDefault(id => (id.ProjectId == model.ProjectId) && (id.EmployeeId == model.EmployeeId));

                _context.RemoveProjectParticipants.Remove(participantExists);
                _context.SaveChanges();

                return Ok();

            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened. " + Ex.Message);
            }

        }

        [HttpPost("resend")]
        public IActionResult ResendKickOffProject([FromBody] ResendKickOffProject model)
        {
            if (model == null)
            {
                return BadRequest("Error, Make sure form is complete!");
            }

            try
            {

                var employee = _context.Employees.FirstOrDefault(id => id.EmployeeId == model.EmployeeId);
                var pp = _context.Projects.FirstOrDefault(id => id.ProjectId == model.ProjectId);

                string url = $"{_configuration["FrontEndUrl"]}/#/CustomSurvey?user={employee.Identifier}&survey={model.ProjectId}";


                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential("web@stratisq.com", "StratisQ@2020"),
                    Timeout = 20000
                };

                using (var message = new MailMessage("web@stratisq.com", employee.Email)
                {
                    IsBodyHtml = true,
                    Subject = "Survey Invitation",
                    Body = "<html><body><p>The project will run from " + pp.StartDate.ToString("dd MMM yyyy") + " to " + pp.EndDate.ToString("dd MMM yyyy") + " please make sure that you complete your survey before the deadline.</p>" +
                    $"<p>To access the survey: <a href='{url}'>Click here</a></p><br><h3>StratisQ Online</h3></body></html>"
                })
                {
                    smtp.Send(message);
                }

                return Ok();
            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened. " + Ex.Message);
            }
        }

        [HttpPost("kickOffProject")]
        public IActionResult KickOffProject([FromBody] KickOffProject model)
        {
            if (model == null)
            {
                return BadRequest("Error, Make sure form is complete!");
            }

            try
            {
                KickOffProject kickOffProject = _context.KickOffProjects.FirstOrDefault(id => id.ProjectId == model.ProjectId);

                if (kickOffProject == null)
                {
                    KickOffProject project = new KickOffProject();

                    project.ProjectId = model.ProjectId;
                    project.Reference = model.Reference;
                    project.DateStamp = DateTime.Now;

                    //Find project and update it's status
                    Project p = _context.Projects.FirstOrDefault(id => id.ProjectId == model.ProjectId);

                    if (p == null)
                    {
                        return BadRequest("Something bad happened. Make sure you are authorized to access this section");
                    }

                    p.IsProjectStarted = true;
                    _context.Projects.Update(p);
                    _context.SaveChanges();

                    //List of participants for this project and send emails
                    List<ProjectParticipant> projectParticipants = _context.ProjectParticipants.Where(id => id.ProjectId == model.ProjectId).ToList();

                    List<int> participantId = new List<int>();

                    foreach (var item in projectParticipants)
                    {
                        //Employees Ids
                        participantId.Add(item.ParticipantId);
                    }

                    List<Employee> employees = new List<Employee>();
                    foreach (var employeeId in participantId)
                    {
                        employees.Add(_context.Employees.FirstOrDefault(id => id.EmployeeId == employeeId));
                    }

                    //Send emails to receipients
                    foreach (var employee in employees)
                    {
                        string url = $"{_configuration["FrontEndUrl"]}/#/CustomSurvey?user={employee.Identifier}&survey={model.ProjectId}";


                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            Credentials = new NetworkCredential("web@stratisq.com", "StratisQ@2020"),
                            Timeout = 20000
                        };

                        Project pp = _context.Projects.FirstOrDefault(id => id.ProjectId == model.ProjectId);

                        using (var message = new MailMessage("web@stratisq.com", employee.Email)
                        {
                            IsBodyHtml = true,
                            Subject = "Survey Invitation",
                            Body = "<html><body><p>The project will run from "+pp.StartDate.ToString("dd MMM yyyy")+" to "+ pp.EndDate.ToString("dd MMM yyyy") + " please make sure that you complete your survey before the deadline.</p>" +
                            $"<p>To access the survey: <a href='{url}'>Click here</a></p></body></html>"
                        })
                        {
                            smtp.Send(message);
                        }
                    }

                }

                return Ok();
            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened! " + Ex.Message);
            }
        }


        [HttpPost("RemoveProjectParticipant")]
        public IActionResult RemoveProjParticipant([FromBody] RemoveProjectParticipant model)
        {
            if (model == null)
            {
                return BadRequest("Error, Make sure form is complete!");
            }

            try
            {
                RemoveProjectParticipant participantExists = _context.RemoveProjectParticipants.FirstOrDefault(id => (id.ProjectId == model.ProjectId) && (id.EmployeeId == model.EmployeeId));

                if (participantExists == null)
                {
                    RemoveProjectParticipant participant = new RemoveProjectParticipant();
                    participant.EmployeeId = model.EmployeeId;
                    participant.ProjectId = model.ProjectId;
                    participant.DateStamp = DateTime.Now;
                    participant.Reference = model.Reference;

                    _context.RemoveProjectParticipants.Add(participant);
                    _context.SaveChanges();
                }

                return Ok();

            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened. " + Ex.Message);
            }

        }

        [HttpPost("")]
        public IActionResult SaveProject([FromBody] Models.Project model)
        {
            if (model == null)
            {
                return BadRequest("Error, Make sure form is complete!");
            }

            try
            {
                Project projectChecker = _context.Projects.FirstOrDefault(id => (id.Name.ToUpper() == model.Name.ToUpper()) && (id.ClientId == model.ClientId));

                if (projectChecker != null)
                {
                    return BadRequest("Project with this name for this client already exists.");
                }

                Project project = new Project();
                project.Name = model.Name;
                project.ClientId = model.ClientId;
                project.TenantId = model.TenantId;
                project.SurveyId = model.SurveyId;
                project.Reference = model.Reference;
                project.DateStamp = model.DateStamp;
                project.FirstReminder = model.FirstReminder.AddHours(model.FirstReminderTime.Hours).AddMinutes(model.FirstReminderTime.Minutes);
                project.SecondReminder = model.SecondReminder.AddHours(model.SecondReminderTime.Hours).AddMinutes(model.SecondReminderTime.Minutes);
                project.Escalate = model.Escalate.AddHours(model.EscalateTime.Hours).AddMinutes(model.EscalateTime.Minutes);
                project.EscalateEmail = model.EscalateEmail;
                project.StartDate = model.StartDate;
                project.EndDate = model.EndDate;
                project.DateStamp = DateTime.Now;

                _context.Projects.Add(project);
                _context.SaveChanges();

                //Add clients participants to the project

                List<ClientEmployee> clientEmployees = _context.ClientEmployees.Where(id => id.ClientId == model.ClientId).ToList();

                foreach (var item in clientEmployees)
                {
                    ProjectParticipant projectParticipant = new ProjectParticipant();
                    projectParticipant.ParticipantId = item.EmployeeId;
                    projectParticipant.ProjectId = project.ProjectId;
                    
                    _context.ProjectParticipants.Add(projectParticipant);
                    _context.SaveChanges();
                }

                return Ok();

            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened! " + Ex.Message);
            }

        }
    }
}
