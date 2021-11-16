using AuthenticationService.Data;
using AuthenticationService.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Services
{
    public interface IAuthenticationServices
    {
        public bool Ping();
        public void SetContextAndConfiguration(AuthenticationServiceContext context, IConfiguration configuration);
        public Task<int> PutAuthentication(string UserName, AuthDTO authentication);
        public string PostAuthentication(AuthDTO authDTO);
        public Task<bool> DeleteAuthentication(string UserName);
        public Task<string> SignUp(MedicSignUpDTO medic);
        public bool AuthenticationExists(string UserName);
    }
}
