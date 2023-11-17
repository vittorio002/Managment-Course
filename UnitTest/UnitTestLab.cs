using Microsoft.AspNetCore.Mvc;
using Data.Controllers;
using LaboratoryApi;

namespace UnitTests
{
    [TestClass]
    public class LabDataControllerTests
    {
        private LabDataController _labDataController;

        [TestInitialize]
        public void Initialize()
        {
            _labDataController = new LabDataController();
        }

        [TestMethod]
        public void LabDataController_Get_ShouldReturnOkResult()
        {
            var result = _labDataController.Get();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void LabDataController_Put_ShouldReturnOkResult()
        {
            var lab = new Lab("LabName");

            var result = _labDataController.Put(lab);

            Assert.IsInstanceOfType(result, typeof(OkResult));

        }

    }
}