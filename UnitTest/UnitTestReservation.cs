using Microsoft.AspNetCore.Mvc;
using Data.Controllers;

namespace UnitTests
{
    [TestClass]
    public class ResDataControllerTests
    {
        private ResDataController _resDataController;

        [TestInitialize]
        public void Initialize()
        {
            _resDataController = new ResDataController();
        }

        [TestMethod]
        public void ResDataController_Get_ShouldReturnOkResult()
        {
            var result = _resDataController.Get();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void ResDataController_Post_ShouldReturnOkResult()
        {
            var reservation = new Reservation("NamePc", "UserName", 9, "31/12/2023");

            var result = _resDataController.Post(reservation);

            Assert.IsInstanceOfType(result, typeof(OkResult));

        }

        [TestMethod]
        public void ResDataController_Delete_ShouldReturnOkResult()
        {
            var reservation = new Reservation("NamePc", "UserName", 9, "31/12/2023");

            var result = _resDataController.Delete(reservation);

            Assert.IsInstanceOfType(result, typeof(OkResult));

        }

    }
}
