using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StratisQAPI.Data;
using StratisQAPI.Entities;

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
    }
}
