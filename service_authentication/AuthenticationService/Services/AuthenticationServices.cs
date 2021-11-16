using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.Data;
using AuthenticationService.DTO;
using DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {

        private AuthenticationServiceContext _context;
        private IConfiguration _configuration;

        public void SetContextAndConfiguration(AuthenticationServiceContext context, IConfiguration configuration )
        {
            if (_context == null )
            {
                _context = context;
            }
            if (_configuration == null)
            {
                _configuration = configuration;
            }
            
        }
        public bool Ping()
        {
            return true;
        }
        public async Task<int> PutAuthentication(string UserName, AuthDTO authentication)
        {
            if (UserName != authentication.UserName)
            {
                return 400; // bad request
            }

            var authentication1 = _context.Authentication.First(e => e.UserName == authentication.UserName);

            if (authentication1 == null)
            {
                return 404; // not found
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
                    return 404; // not found
                }
                else
                {
                    throw;
                }
            }
            return 204; // no content
        }


        public string PostAuthentication(AuthDTO authDTO)
        {
            Authentication authentication = new();
            authentication.UserName = authDTO.UserName;
            authentication.Password = authDTO.Password;
            authentication.EncryptPassword(authentication.Password);
            var auth = _context.Authentication.FirstOrDefault(i => i.UserName == authentication.UserName);
            if (auth == null)
            {
                return "NotFound";
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

                    return bearer_token;
                }
                else
                {
                    return "Unauthorized";
                }
            }
        }

        public async Task<bool> DeleteAuthentication(string UserName)
        {
            var authentication = _context.Authentication.First(e => e.UserName == UserName);
            if (authentication == null)
            {
                return false;
            }

            _context.Authentication.Remove(authentication);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<string> SignUp(MedicSignUpDTO medic)
        {
            Medic newMedic = new();
            User newUser = new();
            Authentication newAuthentication = new();
            newAuthentication.UserName = medic.Authentication.UserName;
            newAuthentication.Password = medic.Authentication.Password;
            if (_context.Authentication.Any(e => e.UserName == newAuthentication.UserName))
            {
                return "Conflict";
            }
            string[] firsName = medic.FirstName.Split(' ');
            string[] lastName = medic.LastName.Split(' ');
            newUser.FirstName = firsName[0];
            newUser.SecondName = firsName[1];
            newUser.Surname = lastName[0];
            newUser.LastName = lastName[1];
            newUser.BirthDate = medic.BirthDate;
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

            return bearer_token;
        }

        public bool AuthenticationExists(string UserName)
        {
            return _context.Authentication.Any(e => e.UserName == UserName);
        }
    }
}
