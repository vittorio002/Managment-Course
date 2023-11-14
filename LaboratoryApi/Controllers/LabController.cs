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
            Lab? lab = _labs.Find(x => x.Name == Name);
            return lab == null ? NotFound() : Ok(lab);
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
        [HttpPost]
        [Route("Computer")]
        public async Task<ActionResult> Post([FromBody] string Labname)
        {
            using (HttpClient request = new HttpClient())
            {
                string apiUrl = "http://localhost:5246/Datapi/LabData/Lab";
                HttpResponseMessage response = request.GetAsync(apiUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    _labs = JsonConvert.DeserializeObject<List<Lab>>(data);
                    Lab lab = _labs.Find(l => l.Name == Labname);
                    int count = 1;
                    foreach (Lab l in _labs)
                    {
                        count += l.computers.Count;
                    }
                    lab.computers.Add(new Computer("Pc" + count));

                    using (HttpClient client = new HttpClient())
                    {
                        string Url = "http://localhost:5246/Datapi/LabData/Lab";

                        string requestData = JsonConvert.SerializeObject(lab);

                        HttpRequestMessage Putrequest = new HttpRequestMessage
                        {
                            Method = HttpMethod.Put,
                            RequestUri = new Uri(Url),
                            Content = new StringContent(requestData, Encoding.UTF8, "application/json")
                        };

                        StringContent content = new StringContent(requestData, Encoding.UTF8, "application/json");

                        HttpResponseMessage Putresponse = await client.SendAsync(Putrequest);

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
                else
                {
                    return BadRequest();
                }
            }
        }
        [HttpPut]
        [Route("Computer")]
        public async Task<ActionResult> PutPc([FromBody] Computer pc)
        {
            using (HttpClient request = new HttpClient())
            {
                string apiUrl = "http://localhost:5246/Datapi/LabData/Lab";
                HttpResponseMessage response = request.GetAsync(apiUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    _labs = JsonConvert.DeserializeObject<List<Lab>>(data);

                    foreach (Lab laboratory in _labs)
                    {
                        Computer? c = laboratory.computers.Find(c => c.Name == pc.Name);

                        if (c != null)
                        {
                            laboratory.computers.Remove(c);
                            c.program = pc.program;
                            c.Status = pc.Status;
                            laboratory.computers.Add(c);

                            using (HttpClient client = new HttpClient())
                            {
                                string Url = "http://localhost:5246/Datapi/LabData/Lab";

                                string requestData = JsonConvert.SerializeObject(laboratory);

                                HttpRequestMessage requestPut = new HttpRequestMessage
                                {
                                    Method = HttpMethod.Put,
                                    RequestUri = new Uri(Url),
                                    Content = new StringContent(requestData, Encoding.UTF8, "application/json")
                                };

                                StringContent content = new StringContent(requestData, Encoding.UTF8, "application/json");

                                HttpResponseMessage responsePut = await client.SendAsync(requestPut);

                                if (response.IsSuccessStatusCode)
                                {
                                    string responseData = await responsePut.Content.ReadAsStringAsync();
                                    return Ok();
                                }
                                else
                                {
                                    return BadRequest();
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            return BadRequest();
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
        [Route("Computer")]
        public async Task<ActionResult> DeletePc([FromBody] string PcName)
        {
            using (HttpClient request = new HttpClient())
            {
                string apiUrl = "http://localhost:5246/Datapi/LabData/Lab";
                HttpResponseMessage response = request.GetAsync(apiUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    _labs = JsonConvert.DeserializeObject<List<Lab>>(data);

                    foreach (Lab lab in _labs)
                    {
                        Computer c = lab.computers.Find(c => c.Name == PcName);
                        if (c != null)
                        {
                            lab.computers.Remove(c);
                            using (HttpClient client = new HttpClient())
                            {
                                string Url = "http://localhost:5246/Datapi/LabData/Lab";

                                string requestData = JsonConvert.SerializeObject(lab);

                                HttpRequestMessage Putrequest = new HttpRequestMessage
                                {
                                    Method = HttpMethod.Put,
                                    RequestUri = new Uri(Url),
                                    Content = new StringContent(requestData, Encoding.UTF8, "application/json")
                                };

                                StringContent content = new StringContent(requestData, Encoding.UTF8, "application/json");

                                HttpResponseMessage Putresponse = await client.SendAsync(Putrequest);

                                if (response.IsSuccessStatusCode)
                                {
                                    string responseData = await response.Content.ReadAsStringAsync();
                                    return Ok();
                                }
                                else
                                {
                                    return BadRequest();
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
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