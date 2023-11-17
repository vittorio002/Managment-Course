using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Requests;
using System.Text;

namespace LaboratoryApi.Controllers
{
    [ApiController]
    [Route("Labapi/[controller]")]
    public class LabController : ControllerBase
    {
        private static List<Lab> _labs = new List<Lab>();

        [HttpGet]
        [TokenValidation]
        public ActionResult<List<Lab>> GetAllLab()
        {
            try
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
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return Ok(_labs);
        }

        [HttpPost]
        [Route("Available")]
        [TokenValidation]
        public async Task<ActionResult> AvailablePc([FromBody] ReserveRequest reserve)
        {
            try
            {
                using (HttpClient request = new HttpClient())
                {
                    string apiUrl = "http://localhost:5246/Datapi/LabData/Lab";
                    HttpResponseMessage response = request.GetAsync(apiUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        _labs = JsonConvert.DeserializeObject<List<Lab>>(data);

                        foreach (Lab lab in _labs)
                        {
                            List<Computer> removePc = new();
                            foreach (Computer pc in lab.computers)
                            {
                                ReserveRequest res = pc.Reserve.Find(r => r.Date == reserve.Date && r.Hour == reserve.Hour);
                                if (res != null)
                                {
                                    removePc.Add(pc);
                                }
                            }
                            foreach (Computer remove in removePc)
                            {
                                lab.computers.Remove(remove);
                            }
                        }
                        return Ok(_labs);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("Computer")]
        [TokenValidation]
        public async Task<ActionResult> PostPc([FromBody] string Labname)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("AddReservation")]
        public async Task<ActionResult> PostReservationOnPc([FromBody] Reservation reservetion)
        {
            try
            {
                using (HttpClient request = new HttpClient())
                {
                    string apiUrl = "http://localhost:5246/Datapi/LabData/Lab";
                    HttpResponseMessage response = request.GetAsync(apiUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        _labs = JsonConvert.DeserializeObject<List<Lab>>(data);

                        foreach (Lab lab in _labs)
                        {
                            Computer pc = lab.computers.Find(c => c.Name == reservetion.NamePc);
                            if (pc != null)
                            {
                                pc.Reserve.Add(new ReserveRequest(reservetion.Date, reservetion.Hour));

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
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return BadRequest();

        }

        [HttpPut]
        [Route("Computer")]
        [TokenValidation]
        public async Task<ActionResult> PutPc([FromBody] Computer pc)
        {
            try
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
                            Computer? c = laboratory.computers.Find(c => c.Id == pc.Id);

                            if (c != null)
                            {
                                laboratory.computers.Remove(c);
                                c.program = pc.program;
                                c.Name = pc.Name;
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
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return BadRequest();
        }

        [HttpPut]
        [TokenValidation]
        public async Task<ActionResult> PutLab(Lab lab)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpDelete]
        [Route("Computer")]
        [TokenValidation]
        public async Task<ActionResult> DeletePc([FromBody] string PcId)
        {
            try
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
                            Computer c = lab.computers.Find(c => c.Id == PcId);
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
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        [Route("RemoveReservation")]
        public async Task<ActionResult> DeleteReservaionOnPc([FromBody] Reservation reservation)
        {
            try
            {
                using (HttpClient request = new HttpClient())
                {
                    string apiUrl = "http://localhost:5246/Datapi/LabData/Lab";
                    HttpResponseMessage response = request.GetAsync(apiUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        _labs = JsonConvert.DeserializeObject<List<Lab>>(data);

                        foreach (Lab lab in _labs)
                        {
                            Computer pc = lab.computers.Find(c => c.Name == reservation.NamePc);
                            if (pc != null)
                            {
                                lab.computers.Remove(pc);
                                ReserveRequest r = pc.Reserve.Find(r => r.Date == reservation.Date && r.Hour == reservation.Hour);
                                pc.Reserve.Remove(r);
                                lab.computers.Add(pc);

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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return BadRequest();
        }

    }
}