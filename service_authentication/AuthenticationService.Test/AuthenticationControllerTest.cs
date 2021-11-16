using AuthenticationService.Controllers;
using AuthenticationService.Data;
using AuthenticationService.DTO;
using AuthenticationService.Services;
using DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AuthenticationService.Test
{
    public class AuthenticationControllerTest
    {
        private readonly Mock<IAuthenticationServices> authenticationMoq;
        private readonly AuthenticationsController _controller;
        private readonly AuthenticationServiceContext contextMoq;
        private readonly Mock<IConfiguration> configurationMoq;

        public AuthenticationControllerTest()
        {
            authenticationMoq = new();
            var options = new DbContextOptionsBuilder<AuthenticationServiceContext>()
            .UseInMemoryDatabase(databaseName: "AwarenessDatabase")
            .Options;
            contextMoq = new(options);
            configurationMoq = new();
            _controller = new(authenticationMoq.Object, contextMoq, configurationMoq.Object);
        }
        [Fact]
        public void OK_Ping()
        {
            ObjectResult objectResult = (ObjectResult)_controller.GetPing();
            Assert.IsType<OkObjectResult>(objectResult as OkObjectResult);
        }

        [Fact]
        public void Error_PostAuthentication_Unauthorized_WrongLoginData()
        {
            authenticationMoq.Setup(e => e.PostAuthentication(It.IsAny<AuthDTO>())).Returns("NotFound");
            AuthDTO auth = new();
            ObjectResult wrongResult = (ObjectResult)_controller.PostAuth(auth);

            Assert.IsType<UnauthorizedObjectResult>(wrongResult as UnauthorizedObjectResult);

            authenticationMoq.Setup(e => e.PostAuthentication(It.IsAny<AuthDTO>())).Returns("Unauthorized");

            Assert.IsType<UnauthorizedObjectResult>(wrongResult as UnauthorizedObjectResult);
        }

        [Fact]
        public async void Error_PutAuthentication_WrongValues()
        {
            authenticationMoq.Setup(e => e.PutAuthentication(It.IsAny<string>(),It.IsAny<AuthDTO>())).ReturnsAsync(400);
            AuthDTO auth = new();
            string Username = "";
            ObjectResult wrongResult = (ObjectResult) await _controller.PutAuthentication(Username ,auth);

            Assert.IsType<BadRequestObjectResult>(wrongResult as BadRequestObjectResult);
        }

        [Fact]
        public async void Error_PutAuthentication_UserNotExist()
        {
            authenticationMoq.Setup(e => e.PutAuthentication(It.IsAny<string>(), It.IsAny<AuthDTO>())).ReturnsAsync(404);
            AuthDTO auth = new();
            string Username = "";
            ObjectResult wrongResult = (ObjectResult)await _controller.PutAuthentication(Username, auth);

            Assert.IsType<NotFoundObjectResult>(wrongResult as NotFoundObjectResult);
        }
        [Fact]
        public async void Ok_PutAuthentication_UserNotExist()
        {
            authenticationMoq.Setup(e => e.PutAuthentication(It.IsAny<string>(), It.IsAny<AuthDTO>())).ReturnsAsync(204);
            AuthDTO auth = new();
            string Username = "";
            NoContentResult wrongResult =(NoContentResult) await _controller.PutAuthentication(Username, auth);

            Assert.IsType<NoContentResult>(wrongResult);
        }

        [Fact]
        public async void Error_DeleteAuthentication_UserDontExist()
        {
            authenticationMoq.Setup(e => e.DeleteAuthentication(It.IsAny<string>())).ReturnsAsync(false);
            string Username = "";
            ObjectResult wrongResult = (ObjectResult) await _controller.DeleteAuthentication(Username);
            Assert.IsType<NotFoundObjectResult>(wrongResult as NotFoundObjectResult);
        }

        [Fact]
        public async void OK_DeleteAuthentication_UserExist()
        {
            authenticationMoq.Setup(e => e.DeleteAuthentication(It.IsAny<string>())).ReturnsAsync(true);
            string Username = "";
            NoContentResult UserExist = (NoContentResult) await _controller.DeleteAuthentication(Username);
            Assert.IsType<NoContentResult>(UserExist);
        }
        [Fact]
        public async void Error_PostMedic_UserAlreadyExist()
        {
            authenticationMoq.Setup(e => e.SignUp(It.IsAny<MedicSignUpDTO>())).ReturnsAsync("Conflict");
            MedicSignUpDTO medic = new();
            ObjectResult wrongResult = (ObjectResult)await _controller.PostMedic(medic);
            Assert.IsType<ConflictObjectResult>(wrongResult as ConflictObjectResult);
        }

    }

}
