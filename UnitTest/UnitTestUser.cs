using Moq;
using Microsoft.AspNetCore.Mvc;
using Requests;
using Data.Controllers;
using Microsoft.AspNetCore.Http;

namespace UnitTests
{
    [TestClass]
    public class UserDataControllerTests
    {
        private UserDataController _userDataController;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;

        [TestInitialize]
        public void Initialize()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _userDataController = new UserDataController();
        }

        [TestMethod]
        public void GetAll_ShouldReturnOkResult()
        {
            var result = _userDataController.GetAll();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void Login_WithValidCredentials_ShouldReturnOkResult()
        {
            var loginRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };
            
            var result = _userDataController.Login(loginRequest);
            
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void Nonce_ShouldReturnOkResult()
        {
            var email = "test@example.com";

            var result = _userDataController.GetNonce(email);
            
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
