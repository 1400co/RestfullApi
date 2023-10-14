using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SocialMedia.Api.Controllers;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Interfaces;
using TransforSerPu.Core.Dtos;
using TransforSerPu.Core.Interfaces;

namespace TestProject.Security
{
    public class TokenControllerTest
    {

        private readonly TokenController _controller;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ISecurityService> _mockSecurityService;
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly Mock<ITokenService> _mockTokenService;

        public TokenControllerTest()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockSecurityService = new Mock<ISecurityService>();
            _mockPasswordService = new Mock<IPasswordService>();
            _mockTokenService = new Mock<ITokenService>();

            // Setup default values (adjust as needed)
            _mockConfiguration.Setup(c => c["Authentication:SecretKey"]).Returns("asdfakjsdfklajsdghflkashdfkhfalskdw24124");
            _mockConfiguration.Setup(c => c["Authentication:Issuer"]).Returns("http://localhost:4848");
            _mockConfiguration.Setup(c => c["Authentication:Audience"]).Returns("http://localhost:4848");

            _controller = new TokenController(_mockConfiguration.Object, _mockSecurityService.Object, _mockPasswordService.Object, _mockTokenService.Object);
        }

        [Fact]
        public async Task GetToken_ReturnsOkResult_WhenUserIsValid()
        {
            // Arrange
            var loginDto = new UserLoginDto { User = "test", Password = "testpass" };
            var securitydto = new SocialMedia.Core.Entities.Security { UserName = "test", Password = "testpass", Role = SocialMedia.Core.Enumerations.RoleType.Administrator };

            _mockSecurityService.Setup(s => s.GetLoginByCredentials(loginDto)).ReturnsAsync(securitydto);
            _mockPasswordService.Setup(p => p.Check(securitydto.Password, loginDto.Password)).Returns(true);

            // Act
            var result = await _controller.GetToken(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value.ToString(); // Assuming your token is returned as a dictionary with a "Token" key.
            Assert.Contains("Token", returnValue);
        }

        [Fact]
        public async Task RenewToken_ReturnsOkResult_WithValidToken()
        {
            // Arrange
            var tokenApiModel = new TokenDto { /* establecer las propiedades del token como corresponda */ };
            var expectedRenewedToken = "tu_token_renovado_aquí";  // Esto debería coincidir con lo que devuelve tu servicio mock

            _mockTokenService.Setup(service => service.RenewToken(It.IsAny<TokenDto>()))
                .ReturnsAsync(new AuthenticatedResponse());

            var controller = new TokenController(_mockConfiguration.Object, _mockSecurityService.Object, _mockPasswordService.Object, _mockTokenService.Object);

            // Act
            var result = await controller.RenewToken(tokenApiModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedRenewedToken, okResult.Value);
        }
    }
}