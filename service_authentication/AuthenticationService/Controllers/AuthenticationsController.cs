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
using Newtonsoft.Json;
using AuthenticationService.DTO;
using AuthenticationService.Services;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IAuthenticationServices _services;
        public AuthenticationsController(IAuthenticationServices authenticationServices, AuthenticationServiceContext context, IConfiguration configuration)
        {
            _services = authenticationServices;
            _services.SetContextAndConfiguration(context, configuration);
        }

        // PING: api/Authentications

        [HttpGet("ping")]
        public ActionResult GetPing()
        {
            try
            {
                _services.Ping();
                return Ok(JsonConvert.SerializeObject("ping"));
            }
            catch (Exception)
            {
                return NotFound(JsonConvert.SerializeObject("Error while doing ping"));
                throw;
            }
        }

        // PUT: api/Authentications/{UserName}
        [Authorize]
        [HttpPut("{UserName}")]
        public async Task<ActionResult> PutAuthentication(string UserName, AuthDTO authentication)
        {
            try
            {
                var response = await _services.PutAuthentication(UserName, authentication);
                if (response == 400)
                {
                    return BadRequest(JsonConvert.SerializeObject("UserName does not match, try again!"));
                }
                else if (response == 404)
                {
                    return NotFound(JsonConvert.SerializeObject("User not exist!"));
                }
                else if (response == 204)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return BadRequest(JsonConvert.SerializeObject("Error during authentication, try again later"));
                throw;
            }
        }

        // POST: api/Authentications/auth

        [HttpPost("Auth")]
        public ActionResult PostAuth(AuthDTO authDTO)
        {
            try
            {
                var result = _services.PostAuthentication(authDTO);

                if (result.Equals("NotFound"))
                {
                    return Unauthorized(JsonConvert.SerializeObject("Username or Password Incorrect!"));
                }
                else if (result.Equals("Unauthorized"))
                {
                    return Unauthorized(JsonConvert.SerializeObject("Username or Password Incorrect!"));
                }
                else
                {
                    return Ok(JsonConvert.SerializeObject(result));
                }


            }
            catch (Exception)
            {
                return BadRequest(JsonConvert.SerializeObject("Error during authentication, try again later"));
                throw;
            }

        }

        // DELETE: api/Authentications/{UserName}
        [Authorize]
        [HttpDelete("{UserName}")]
        public async Task<ActionResult> DeleteAuthentication(string UserName)
        {
            try
            {
                var result = await _services.DeleteAuthentication(UserName);
                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound(JsonConvert.SerializeObject("User with username: " + UserName + " Does not exist!"));
                }
            }
            catch (Exception)
            {
                return BadRequest(JsonConvert.SerializeObject("Bad Request, try again later"));
                throw;
            }
        }

        // POST: api/Authentications/SignUp

        [HttpPost("SignUp")]
        public async Task<ActionResult> PostMedic(MedicSignUpDTO medic)
        {
            try
            {
                var result = await _services.SignUp(medic);
                if (result.Equals("Conflict"))
                {
                    return Conflict(JsonConvert.SerializeObject("User Already Exist!"));
                }
                else
                {
                    return Ok(JsonConvert.SerializeObject(result));
                }
            }
            catch (Exception)
            {
                return NotFound(JsonConvert.SerializeObject("Service has an internal error, try again later!"));
                throw;

            }
        }
    }
}
