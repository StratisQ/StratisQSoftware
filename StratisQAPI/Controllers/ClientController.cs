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
    public class ClientController : Controller
    {
        private readonly StratisQDbContextUsers _contextUsers;
        private readonly StratisQDbContext _context;
        public ClientController(StratisQDbContext context, StratisQDbContextUsers contextUsers)
        {
            _context = context;
            _contextUsers = contextUsers;
        }

        [HttpGet("industries")]
        public IActionResult Industries()
        {
            List<Industry> industries = _context.Industries.ToList();

            return Ok(industries);
        }

        [HttpGet("")]
        public IActionResult Clients(string reference)
        {
            
            var user = _contextUsers.Users.FirstOrDefault(id => id.Id == reference);

            int tenantId = user.TenantId;

            List<Client> lClients = _context.Clients.Where(id=>id.TenantId == tenantId).ToList();

            var clients = lClients.Select(result => new 
            {
                Name = result.Name,
                Date = result.DateStamp.ToString("dd MMM yyyy"),
                Industry = _context.Industries.FirstOrDefault(id=>id.IndustryId == result.IndustryId).IndustryName,
                Website = result.Website,
                Email = result.Email,
                Telephone = result.TelephoneNumber,
                RegistrationNumber = result.RegistrationNumber,
                VatNumber = result.VatNumber,
                ClientId = result.ClientId,
                TenantId = result.TenantId
            });

            return Ok(clients);
        }

        [HttpGet("biographicDetails")]
        public IActionResult GetBiographicDetails(int biographicId)
        {
            if (biographicId == 0)
            {
                return BadRequest("Make sure the client exists.");
            }

            List<BiographicDetail> biographics = _context.BiographicDetails.Where(id => id.BiographicId == biographicId).ToList();

            return Ok(biographics);
        }

        [HttpGet("biographic")]
        public IActionResult GetBiographic(int clientId)
        {
            if (clientId == 0)
            {
                return BadRequest("Make sure the client exists.");
            }

            List<Biographic> biographics =  _context.Biographics.Where(id => id.ClientId == clientId).ToList();

            return Ok(biographics);
        }


        [HttpPost("biographicDetail")]
        public IActionResult SaveBiographicType([FromBody] BiographicDetailModel model)
        {
            if (model == null)
            {
                return BadRequest("Error, Make sure form is complete!");
            }

            try
            {
                BiographicDetail biographicChecker = _context.BiographicDetails.FirstOrDefault(id => (id.Name.ToUpper() == model.Name.ToUpper()) && (id.BiographicId == model.BiographicId));

                if (biographicChecker != null)
                {
                    return BadRequest("Biographic detail with this name for this biographic type already exists.");
                }

                BiographicDetail biographicDetail = new BiographicDetail();
                biographicDetail.BiographicId = model.BiographicId;
                biographicDetail.Name = model.Name;
                biographicDetail.ClientId = model.ClientId;
                biographicDetail.TenantId = model.TenantId;
                biographicDetail.Reference = model.Reference;
                biographicDetail.DateStamp = DateTime.Now;

                _context.BiographicDetails.Add(biographicDetail);
                _context.SaveChanges();

                return Ok();

            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened! " + Ex.Message);
            }

        }

        [HttpPost("biographic")]
        public IActionResult SaveBiographic([FromBody] BiographicModel model)
        {
            if (model == null)
            {
                return BadRequest("Error, Make sure form is complete!");
            }

            try
            {
                Biographic biographicChecker = _context.Biographics.FirstOrDefault(id => (id.Name.ToUpper() == model.Name.ToUpper()) && (id.ClientId == model.ClientId));

                if (biographicChecker != null)
                {
                    return BadRequest("Biographic Type with this name for this client already exists.");
                }

                Biographic biographic = new Biographic();
                biographic.Name = model.Name;
                biographic.ClientId = model.ClientId;
                biographic.TenantId = model.TenantId;
                biographic.Reference = model.Reference;
                biographic.DateStamp = DateTime.Now;

                _context.Biographics.Add(biographic);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened! "+Ex.Message);
            }

        }

        [HttpPost("")]
        public IActionResult SaveClient([FromBody] Client model)
        {
            if (model == null)
            {
                return BadRequest("Error, Make sure form is complete!");
            }

            //Check if client exists
            Client client1 = _context.Clients.FirstOrDefault(id => (id.Name == model.Name) && (id.RegistrationNumber == model.RegistrationNumber));

            if (client1 != null)
            {
                return BadRequest("There is already an existing client with this name: " + model.Name + " and registration number: " + model.RegistrationNumber);
            }

            try
            {
                var user = _contextUsers.Users.FirstOrDefault(id => id.Id == model.Reference);

                int tenantId = user.TenantId;

                Client client = new Client();
                client.DateStamp = DateTime.Now;
                client.Email = model.Email.ToLower();
                client.IndustryId = model.IndustryId;
                client.Name = model.Name;
                client.Notes = model.Notes;
                client.Reference = model.Reference;
                client.RegistrationNumber = model.RegistrationNumber;
                client.TelephoneNumber = model.TelephoneNumber;
                client.TenantId = tenantId;
                client.VatNumber = model.VatNumber;
                client.Website = model.Website;

                _context.Clients.Add(client);
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
