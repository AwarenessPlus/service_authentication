using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.Data;
using DomainModel;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
<<<<<<< HEAD
using Newtonsoft.Json;
using AuthenticationService.DTO;
=======
<<<<<<< Updated upstream
=======
using Newtonsoft.Json;
using AuthenticationService.DTO;
using DatabaseServices.DTO;
>>>>>>> Stashed changes
>>>>>>> feature/AuthenticationService

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly AuthenticationServiceContext _context;
        private readonly IConfiguration _configuration;
      
        public AuthenticationsController(AuthenticationServiceContext context, IConfiguration config)
        {
            _context = context;
            _configuration = config;
        }

<<<<<<< HEAD
        // PING: api/Authentications

        [HttpGet]
        public ActionResult GetPing()
        {
            return Ok();
        }


=======
<<<<<<< Updated upstream
>>>>>>> feature/AuthenticationService
        // GET: api/Authentications/5
=======
        // PING: api/Authentications

        [HttpGet("ping")]
        public ActionResult GetPing()
        {
            return Ok();
        }

        // PUT: api/Authentications/{UserName}
>>>>>>> Stashed changes
        [Authorize]
        [HttpPut("{UserName}")]
        public async Task<IActionResult> PutAuthentication(string UserName, AuthDTO authentication)
        {
            if (UserName != authentication.UserName)
            {
                return BadRequest();
            }

            var authentication1 = _context.Authentication.First(e => e.UserName == authentication.UserName);
            
            if (authentication1 == null)
            {
                return NotFound();
            }
            else
            {
                authentication1.Password = authentication.Password;
                authentication1.EncryptPassword(authentication1.Password);
            }

            _context.Entry(authentication1).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthenticationExists(UserName))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Authentications/auth

        [HttpPost("Auth")]
        public ActionResult<Authentication> PostAuth(AuthDTO authDTO)
        {
            Authentication authentication = new();
            authentication.UserName = authDTO.UserName;
            authentication.Password = authDTO.Password;
            authentication.EncryptPassword(authentication.Password);
            var auth = _context.Authentication.FirstOrDefault(i => i.UserName == authentication.UserName);
            if (auth == null)
            {
                return NotFound();
            }
            else
            {
                if (auth.Password.Equals(authentication.Password))
                {
                    var secretKey = _configuration.GetValue<String>("AppSecretKey");
                    var key = Encoding.ASCII.GetBytes(secretKey);

                    var claims = new ClaimsIdentity();
                    claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, authentication.UserName));

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = claims,
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var createdToken = tokenHandler.CreateToken(tokenDescriptor);
                    string bearer_token = tokenHandler.WriteToken(createdToken);

                    return Ok(JsonConvert.SerializeObject(bearer_token));
                }
                else
                {
                    return Unauthorized();
                }
            }
            
        }

        // DELETE: api/Authentications/{UserName}

        [Authorize]
        [HttpDelete("{UserName}")]
        public async Task<IActionResult> DeleteAuthentication(string UserName)
        {
            var authentication = _context.Authentication.First(e => e.UserName == UserName);
            if (authentication == null)
            {
                return NotFound();
            }

            _context.Authentication.Remove(authentication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Authentications/SignUp

        [HttpPost("SignUp")]
        public async Task<ActionResult<Medic>> PostMedic(MedicDTO medic)
        {
            Medic newMedic = new();
            User newUser = new();
            Authentication newAuthentication = new();
            newAuthentication.UserName = medic.Authentication.UserName;
            newAuthentication.Password = medic.Authentication.Password;
            if (_context.Authentication.Any(e => e.UserName == newAuthentication.UserName))
            {
                return Conflict(JsonConvert.SerializeObject("User Already Exist"));
            }
            string[] firsName = medic.FirstName.Split(' ');
            string[] lastName = medic.LastName.Split(' ');
            newUser.FirstName = firsName[0];
            newUser.SecondName = firsName[1];
            newUser.Surname = lastName[0];
            newUser.LastName = lastName[1];
            newUser.Age = medic.Age;
            newMedic.MedicData = newUser;
            newMedic.Rotation = medic.Rotation;
            newMedic.Semester = medic.Semester;
            newMedic.AuthenticationData = newAuthentication;
            newMedic.AuthenticationData.EncryptPassword(newMedic.AuthenticationData.Password);
            _context.Medic.Add(newMedic);
            await _context.SaveChangesAsync();
            //Construct The Bearer JSON Web Token
            var secretKey = _configuration.GetValue<String>("AppSecretKey");
            var key = Encoding.ASCII.GetBytes(secretKey);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, newMedic.AuthenticationData.UserName));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);
            string bearer_token = tokenHandler.WriteToken(createdToken);

            return Ok(JsonConvert.SerializeObject(bearer_token));

        }

        private bool AuthenticationExists(string UserName)
        {
            return _context.Authentication.Any(e => e.UserName == UserName);
        }
    }
}
