using FinalAuthProject.Controllers;
using FinalAuthProject.Models;
using FinalAuthProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FinalAuthProject.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<JwtService> _jwtServiceMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                null, null, null, null, null, null, null , null
            );
            _configurationMock = new Mock<IConfiguration>();
            _jwtServiceMock = new Mock<JwtService>();
            _loggerMock = new Mock<ILogger<AuthController>>();

            _authController = new AuthController(
                _userManagerMock.Object,
                _configurationMock.Object,
                _jwtServiceMock.Object,
                _loggerMock.Object
            );
        }

        // Existing test methods...
    }
}
