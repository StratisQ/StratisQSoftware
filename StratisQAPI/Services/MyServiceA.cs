using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StratisQAPI.Data;
using StratisQAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace StratisQAPI.Services
{
    public class MyServiceA : BackgroundService
    {
        public IServiceProvider Services { get; }

        private readonly IConfiguration _configuration;

        public MyServiceA(IServiceProvider services, IConfiguration configuration)
        {
            Services = services;
            _configuration = configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stopToken)
        {
            while (!stopToken.IsCancellationRequested)
            {
                Console.WriteLine("Reminder's checker");

                await Task.Delay(TimeSpan.FromSeconds(60));

                await EmailBackground(stopToken);
            }
        }

        private async Task EmailBackground(CancellationToken stoppingToken)
        {

            using (var scope = Services.CreateScope())
            {
                var _context =
                    scope.ServiceProvider
                        .GetRequiredService<StratisQDbContext>();


                //First Reminder
                List<Project> firstReminderProjects = _context.Projects.Where(id => (id.IsFirstReminderSent == false) && (id.IsProjectStarted == true) &&
                ((id.FirstReminder >= DateTime.Now.AddMinutes(-1) && (id.FirstReminder <= DateTime.Now.AddMinutes(1))))).ToList();


                foreach (Project project in firstReminderProjects)
                {

                    List<ProjectParticipant> projectParticipants = _context.ProjectParticipants.Where(id => (id.ProjectId == project.ProjectId) && (id.IsParticipated == false)).ToList();
                    List<Employee> employees = new List<Employee>();

                    foreach (var item in projectParticipants)
                    {
                        employees.Add(_context.Employees.FirstOrDefault(id => id.EmployeeId == item.ParticipantId));
                    }

                    foreach (Employee employee in employees)
                    {
                        Employee emp = _context.Employees.FirstOrDefault(id => id.EmployeeId == employee.EmployeeId);

                        string url = $"{_configuration["FrontEndUrl"]}/#/CustomSurvey?user={employee.Identifier}&survey={project.ProjectIdentifier}";

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
                            Subject = "Survey Invitation: First Reminder",
                            Body = "<html><body><p>Hi, " + employee.Name + ", You have not attended this survey which runs from " + project.StartDate.ToString("dd MMM yyyy") + " to " + project.EndDate.ToString("dd MMM yyyy") + ". Please make sure that you complete your survey before the project closes.</p>" +
                            $"<p>To access the survey: <a href='{url}'>Click here</a></p><br><h3>StratisQ Online</h3></body></html>"
                        })
                        {
                            smtp.Send(message);
                        }
                    }

                    project.IsFirstReminderSent = true;
                    _context.Projects.Update(project);
                    _context.SaveChanges();
                }

                //Second reminder
                List<Project> secondReminderProjects = _context.Projects.Where(id => (id.IsFirstReminderSent == true) && (id.IsSecondReminderSent == false) && (id.IsEscalateSent == false) && (id.IsProjectStarted == true) &&
                ((id.SecondReminder >= DateTime.Now.AddMinutes(-1) && (id.SecondReminder <= DateTime.Now.AddMinutes(1))))).ToList();


                foreach (Project project in secondReminderProjects)
                {

                    List<ProjectParticipant> projectParticipants = _context.ProjectParticipants.Where(id => (id.ProjectId == project.ProjectId) && (id.IsParticipated == false)).ToList();
                    List<Employee> employees = new List<Employee>();

                    foreach (var item in projectParticipants)
                    {
                        employees.Add(_context.Employees.FirstOrDefault(id => id.EmployeeId == item.ParticipantId));
                    }

                    foreach (Employee employee in employees)
                    {
                        Employee emp = _context.Employees.FirstOrDefault(id => id.EmployeeId == employee.EmployeeId);

                        string url = $"{_configuration["FrontEndUrl"]}/#/CustomSurvey?user={employee.Identifier}&survey={project.ProjectIdentifier}";

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
                            Subject = "Survey Invitation: Second Reminder",
                            Body = "<html><body><p>Hi, " + employee.Name + ", You have not attended this survey, including first reminder. This project runs from " + project.StartDate.ToString("dd MMM yyyy") + " to " + project.EndDate.ToString("dd MMM yyyy") + ". Please make sure that you complete your survey before the project closes.</p>" +
                            $"<p>To access the survey: <a href='{url}'>Click here</a></p><br><h3>StratisQ Online</h3></body></html>"
                        })
                        {
                            smtp.Send(message);
                        }
                    }

                    project.IsSecondReminderSent = true;
                    _context.Projects.Update(project);
                    _context.SaveChanges();
                }


                //Escalete reminder
                List<Project> escalateProjects = _context.Projects.Where(id => (id.IsFirstReminderSent == true) && (id.IsSecondReminderSent == true) && (id.IsEscalateSent == false) && (id.IsProjectStarted == true) &&
                ((id.Escalate >= DateTime.Now.AddMinutes(-1) && (id.Escalate <= DateTime.Now.AddMinutes(1))))).ToList();


                foreach (Project project in escalateProjects)
                {

                    List<ProjectParticipant> projectParticipants = _context.ProjectParticipants.Where(id => (id.ProjectId == project.ProjectId) && (id.IsParticipated == false)).ToList();
                    List<Employee> employees = new List<Employee>();

                    foreach (var item in projectParticipants)
                    {
                        employees.Add(_context.Employees.FirstOrDefault(id => id.EmployeeId == item.ParticipantId));
                    }

                    foreach (Employee employee in employees)
                    {
                        Employee emp = _context.Employees.FirstOrDefault(id => id.EmployeeId == employee.EmployeeId);

                        string url = $"{_configuration["FrontEndUrl"]}/#/CustomSurvey?user={employee.Identifier}&survey={project.ProjectIdentifier}";

                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            Credentials = new NetworkCredential("web@stratisq.com", "StratisQ@2020"),
                            Timeout = 20000
                        };


                        using (var message = new MailMessage("web@stratisq.com", project.EscalateEmail)
                        {
                            IsBodyHtml = true,
                            Subject = "Survey Invite Escalation",
                            Body = "<html><body><p>Hi, Your team member: "+employee.Name+" "+employee.Surname+" ("+employee.Email+")"+ " have not attended the project. He/She has furthure ignored the first and second reminders. This project runs from " + project.StartDate.ToString("dd MMM yyyy") + " to " + project.EndDate.ToString("dd MMM yyyy") + ". Please make sure that you communicate with him/her before the project closes.</p>" +
                            $"<br><h3>StratisQ Online</h3></body></html>"
                        })
                        {
                            smtp.Send(message);
                        }
                    }

                    project.IsEscalateSent = true;
                    _context.Projects.Update(project);
                    _context.SaveChanges();
                }


            }
        }
       
    }
}
