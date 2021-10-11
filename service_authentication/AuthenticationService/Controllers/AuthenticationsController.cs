using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.Data;
using DomainModel;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.JsonWebTokens;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using AuthenticationService.DTO;

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

        // PING: api/Authentications

        [HttpGet]
        public ActionResult GetPing()
        {
            return Ok();
        }


        // GET: api/Authentications/5
        [Authorize]
        [HttpGet("{UserName}")]
        public ActionResult<Authentication> GetAuthentication(String UserName)
        {
            var authentication = _context.Authentication.FirstOrDefault(i => i.UserName == UserName);

            if (authentication == null)
            {
                return NotFound();
            }

            return authentication;
        }

        // PUT: api/Authentications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthentication(int id, Authentication authentication)
        {
            if (id != authentication.AuthenticationID)
            {
                return BadRequest();
            }

            _context.Entry(authentication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthenticationExists(id))
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

        // POST: api/Authentications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Authentication>> PostAuthentication(Authentication authentication)
        {
            authentication.EncryptPassword(authentication.Password);
            _context.Authentication.Add(authentication);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthentication", new { id = authentication.AuthenticationID }, authentication);
        }

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

        // DELETE: api/Authentications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthentication(int id)
        {
            var authentication = await _context.Authentication.FindAsync(id);
            if (authentication == null)
            {
                return NotFound();
            }

            _context.Authentication.Remove(authentication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthenticationExists(int id)
        {
            return _context.Authentication.Any(e => e.AuthenticationID == id);
        }
    }
}
