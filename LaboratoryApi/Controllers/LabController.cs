using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LaboratoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabController : ControllerBase
    {
        private static List<Lab> _labs = new List<Lab>();

    }
}