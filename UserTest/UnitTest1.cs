using Azure;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.IServices;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading.Tasks;

namespace UserTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task Login_NonExistingEmail_Returns400Status_Mocking()
        {
            Mock<IAuthService> mockAuthService = new Mock<IAuthService>();

            var input = new LoginDto()
            {
                Email = "Ali@yahoo.com",
                Password = "123456ass@!DD"
            };

            var output = new GenericResponseDto<AuthDto>()
            {
                Message = "Email or Password is incorrect!",
                Status = 400,
            };

            mockAuthService.Setup(m => m.Login(input)).ReturnsAsync(output);

            var actualResponse = await mockAuthService.Object.Login(input);

            Assert.Equal(400, actualResponse.Status);
        }

        [Fact]
        public async Task Login_WrongPassword_Returns400Status_Mocking()
        {
            Mock<IAuthService> mockAuthService = new Mock<IAuthService>();

            var input = new LoginDto()
            {
                Email = "Ali@yahoo.com",
                Password = "123456ass@!DD"
            };

            var output = new GenericResponseDto<AuthDto>()
            {
                Message = "Email or Password is incorrect!",
                Status = 400,
            };

            mockAuthService.Setup(m => m.Login(input)).ReturnsAsync(output);

            var actualResponse = await mockAuthService.Object.Login(input);

            Assert.Equal(400, actualResponse.Status);
        }


        [Fact]
        public async Task RefreshToken_NotFoundRefreshToken_ReturnsInvalidTokenWith400Status_Mocking()
        {

            Mock<IAuthService> mockAuthService = new Mock<IAuthService>();

            var input = "wOEu4wJzM9Q2Zhc3FO2zIbzrcazIcFrU";

            var expectedResponse = new GenericResponseDto<AuthDto>()
            {
                Message = "Invalid Token",
                Status = 400,
            };

            mockAuthService.Setup(s => s.RefreshTokenAsync(input)).ReturnsAsync(expectedResponse);

            var result = mockAuthService.Object.RefreshTokenAsync(input);

            Assert.Equal("Invalid Token", expectedResponse.Message);
            Assert.Equal(400, expectedResponse.Status);

        }

    }

}
