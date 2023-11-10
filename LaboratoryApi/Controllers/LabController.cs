using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace LaboratoryApi.Controllers
{
    [ApiController]
    [Route("Labapi/[controller]")]
    public class LabController : ControllerBase
    {
        private static List<Lab> _labs = new List<Lab>();

        [HttpGet]
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
        
        [HttpGet]
        [Route("{Name}")]
        public ActionResult<Lab> Get(string Name)
        {
            Lab? user = _labs.Find(x => x.Name == Name);      
            return user == null ? NotFound() : Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult> Post(Lab lab)
        {
            Lab? existingUser = _labs.Find(x => x.Name == lab.Name);
            if (existingUser != null)
            {
                return Conflict("Cannot create the User … exists.");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    string Url = "http://localhost:5246/Datapi/LabData/Lab";

                    string requestData = JsonConvert.SerializeObject(lab);

                    StringContent content = new StringContent(requestData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(Url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                var resourceUrl = Request.Path.ToString() + '/' + lab.Name;
                return Created(resourceUrl, lab);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Put(Lab lab)
        {
            Lab? existingUser = _labs.Find(x => x.Name == lab.Name);
            if (existingUser == null)
            {
                return BadRequest("Cannot update … term.");
            }
            else
            {
                 using (HttpClient client = new HttpClient())
                {
                    string Url = "http://localhost:5246/Datapi/LabData/Lab";

                    string requestData = JsonConvert.SerializeObject(lab);

                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Put,
                        RequestUri = new Uri(Url),
                        Content = new StringContent(requestData, Encoding.UTF8, "application/json")
                    };

                    StringContent content = new StringContent(requestData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return Ok();
            }
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(string Name)
        {
            /*User? user = _users.Find(x => x.Email == Email);

            if
            (user == null)
            {
                return NotFound();
            }
            else
            {*/
                
                using (HttpClient client = new HttpClient())
                {
                    string Url = "http://localhost:5246/Datapi/LabData/Lab";

                    string requestData = JsonConvert.SerializeObject(Name);

                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(Url),
                        Content = new StringContent(requestData, Encoding.UTF8, "application/json")
                    };

                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return NoContent();
            //}
        }
    }
}