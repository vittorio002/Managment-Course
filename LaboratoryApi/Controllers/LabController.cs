using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LaboratoryApi.Controllers
{
    [ApiController]
    [Route("Labapi/[controller]")]
    public class LabController : ControllerBase
    {
        private static List<Lab> _labs = new List<Lab>();

        [HttpGet("Lab")]
        public ActionResult<List<Lab>> Get()
        {
            using (HttpClient request = new HttpClient())
            {
                string apiUrl = "http://localhost:5246/Datapi/LabData/Lab";
                HttpResponseMessage response = request.GetAsync(apiUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    _labs = JsonConvert.DeserializeObject<List<Lab>>(data);              
                }
            }
            return Ok(_labs);
        }
    }
}