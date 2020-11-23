using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StratisQAPI.Data;
using StratisQAPI.Entities;
using StratisQAPI.Models;

namespace StratisQAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly StratisQDbContextUsers _contextUsers;
        private readonly StratisQDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;

        public AccountController(StratisQDbContext context, StratisQDbContextUsers contextUsers,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _contextUsers = contextUsers;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        [HttpGet("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    return BadRequest("No user associated with email");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = Encoding.UTF8.GetBytes(token);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);

                string url = $"{_configuration["FrontEndUrl"]}/#/ResetPassword?email={email}&token={validToken}";


                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential("web@stratisq.com", "StratisQ@2020"),
                    Timeout = 20000
                };

                using (var message = new MailMessage("web@stratisq.com", email)
                {
                    IsBodyHtml = true,
                    Subject = "StratisQ: Password Reset Instruction",
                    Body = "<html><body><h1>Follow the instructions to reset your password</h1>" +
                    $"<p>To reset your password <a href='{url}'>Click here</a></p></body></html>"
                })
                {
                    smtp.Send(message);
                }

                return Ok();
            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened! " + Ex.Message);
            }

        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return BadRequest("No user associated with email");
                }

                if (model.NewPassword != model.ConfirmPassword)
                {
                    return BadRequest("Password doesn't match its confirmation");
                }

                var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
                string normalToken = Encoding.UTF8.GetString(decodedToken);

                var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);

                if (result.Succeeded)
                {
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Credentials = new NetworkCredential("web@stratisq.com", "StratisQ@2020"),
                        Timeout = 20000
                    };

                    using (var message = new MailMessage("web@stratisq.com", model.Email)
                    {
                        IsBodyHtml = true,
                        Subject = "StratisQ Software System: Password Reset",
                        Body = "<html><body><h1>Hi, you have successfully changed your password.</h1></body></html>"
                    })
                    {
                        smtp.Send(message);
                    }
                }
                else
                {
                    return BadRequest("Error, Expired request. Please try again, start by clicking on forgot password");
                }

                return Ok(result);
            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened. " + Ex.Message);
            }

        }

        [HttpGet("countries")]
        public IActionResult GetCountries()
        {
            try
            {
                return Ok(_context.Countries.ToList());
            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened! " + Ex.Message);
            }
        }

        [HttpGet("tenants")]
        public IActionResult GetTenants()
        {
            try
            {
                List<Tenant> lTenants = _contextUsers.Tenants.ToList();

                var tenants = lTenants.Select(result => new
                {
                    TenantId = result.TenantId,
                    TenantKey = result.TenantKey,
                    TenantName = result.TenantName,
                    DateStamp = result.DateStamp.ToString("dd MMMM yyyy")
                });

                return Ok(tenants);
            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened! " + Ex.Message);
            }
        }

        [HttpPost("tenants")]
        public IActionResult SaveTenant([FromBody] TenantModel model)
        {
            if (model == null)
            {
                return BadRequest("Error, Make sure tenant form is complete!");
            }
            try
            {
                Tenant tenant = _contextUsers.Tenants.FirstOrDefault(id => id.TenantKey == model.TenantKey);

                if (tenant != null)
                {
                    return BadRequest("Tenant with the key: " + model.TenantKey + " already exists");
                }

                Tenant t = new Tenant();
                t.DateStamp = DateTime.Now;
                t.TenantKey = model.TenantKey;
                t.TenantName = model.TenantName;
                t.Reference = model.Reference;

                _contextUsers.Tenants.Add(t);
                _contextUsers.SaveChanges();

                

                return Ok();
            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened! " + Ex.Message);
            }
        }

        [HttpGet("deleteId")]
        public IActionResult DeleteTenant(int tenantId)
        {

            try
            {
                Tenant tenant = _contextUsers.Tenants.FirstOrDefault(id => id.TenantId == tenantId);

                return Ok();
            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened! " + Ex.Message);
            }
        }

        [HttpGet("getTenantUsers")]
        public IActionResult GetTenantAdmins(int tenantId)
        {

            try
            {
                var tenantAdmins = _contextUsers.Users.Where(id => (id.TenantId == tenantId) && (id.Email.ToUpper() != "web@stratisq.com".ToUpper()));

                return Ok(tenantAdmins);
            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened! " + Ex.Message);
            }
        }

        [HttpPost("addTenantUser")]
        public IActionResult SaveTenantUser([FromBody] TenantUser model)
        {
            string password = CreatePassword(6);
            string url = $"{_configuration["FrontEndUrl"]}";

            if (model == null)
            {
                return BadRequest("Error, Make sure tenant form is complete!");
            }

            ApplicationUser userChecker = _contextUsers.Users.FirstOrDefault(id => id.Email == model.Email);

            if (userChecker != null)
            {
                return BadRequest("Error, User with this email already exists. Contact administrator.");
            }

            try
            {
                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.FirstName = model.FirstName;
                applicationUser.LastName = model.LastName;
                applicationUser.Email = model.Email;
                applicationUser.UserName = model.Email;
                applicationUser.Reference = model.Reference;
                applicationUser.DateStamp = DateTime.Now;
                applicationUser.TenantId = model.TenantId;

                IdentityResult result = _userManager.CreateAsync(applicationUser, password).Result;

                _context.SaveChanges();

                _userManager.AddToRoleAsync(applicationUser, "ADMINISTRATOR");
                _contextUsers.SaveChanges();

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential("web@stratisq.com", "StratisQ@2020"),
                    Timeout = 20000
                };

                using (var message = new MailMessage("web@stratisq.com", model.Email)
                {
                    IsBodyHtml = true,
                    Subject = "StratisQ: "+_contextUsers.Tenants.FirstOrDefault(id=>id.TenantId == model.TenantId).TenantKey+" Account" ,
                    Body = "<html><body>Hi " + applicationUser.FirstName + ", <br/>Your account has created sucessfully: <br/><br/>Link: "+url+ "<br/><br/>Username: " + applicationUser.Email + "<br/>Password: " + password + " <br/><br/><br/><p>StratisQ</p></body></html></body></html>"
                })
                {
                    smtp.Send(message);
                }

                return Ok();

            }
            catch (Exception Ex)
            {
                return BadRequest("Something bad happened! " + Ex.Message);
            }
        }


        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] Login model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                //Find if user belongs to a tenant

                Tenant tenant = _contextUsers.Tenants.FirstOrDefault(id => id.TenantKey.ToUpper() == model.Tenant.ToUpper());

                if (tenant == null)
                {
                    return BadRequest("Tenant does not exist, contact your administrator!");
                }

                if (tenant.TenantKey.ToUpper() != model.Tenant.ToUpper())
                {
                    return BadRequest("You do not belong to this tenant!");
                }

                if (user != null)
                {
                    if (user.IsLocked)
                    {
                        return BadRequest("User is currently locked, ask administrator to unlock you!");
                    }

                    if (user.IsDeleted)
                    {
                        return BadRequest("User is deleted, ask administrator for reinstatement!");
                    }

                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Id ),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                            new Claim(JwtRegisteredClaimNames.Email, user.UserName)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersedkjhulfgyuerfw344cret"));

                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            issuer: "https://stratisq.co.za:81",
                            audience: "https://stratisq.co.za:80",
                            claims: claims,
                            expires: DateTime.UtcNow.AddDays(1),
                            signingCredentials: creds);

                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            userId = token.Subject,
                            username = user.UserName,
                            firstName = user.FirstName,
                            lastName = user.LastName,
                            isLocked = user.IsLocked,
                            //role = _userManager.GetRolesAsync(user),
                            Tenant = _contextUsers.Tenants.FirstOrDefault(IdentityBuilder=>IdentityBuilder.TenantId == user.TenantId).TenantKey
                        };

                        return Created("", results);
                    }
                    else
                    {
                        return BadRequest("Login Failed, Wrong Username/Password");
                    }
                }
                else
                {
                    return BadRequest("Account Does Not Exist, Please Register First");
                }
            }
            return BadRequest("Login Failed");
        }

    }
}
