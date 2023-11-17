using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

[ApiController]
[Route("Reservation/[controller]")]
[TokenValidation]
public class ReservationController : ControllerBase
{
    private static List<Reservation> _reservation = new();

    [HttpGet]
    public async Task<ActionResult<List<Reservation>>> GetAll()
    {
        try
        {
            using (HttpClient request = new HttpClient())
            {
                string apiUrl = "http://localhost:5246/Datapi/resData/Reservation";
                HttpResponseMessage response = request.GetAsync(apiUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    _reservation = JsonConvert.DeserializeObject<List<Reservation>>(data);
                }
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
        return Ok(_reservation);
    }
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Reservation reservation)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string Url = "http://localhost:5246/Datapi/resData/Reservation";

                string requestData = JsonConvert.SerializeObject(reservation);

                StringContent content = new StringContent(requestData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(Url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    using (HttpClient PcAddR = new HttpClient())
                    {
                        string Url2 = "http://localhost:5033/Labapi/Lab/AddReservation";

                        string Data = JsonConvert.SerializeObject(reservation);

                        StringContent content2 = new StringContent(Data, Encoding.UTF8, "application/json");

                        HttpResponseMessage response2 = await client.PostAsync(Url2, content2);

                        if (response2.IsSuccessStatusCode)
                        {
                            string responseData2 = await response2.Content.ReadAsStringAsync();
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
    [Route("Specific")]
    public async Task<ActionResult> GetUserReservation([FromBody] string email)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "http://localhost:5246/Datapi/resData/Reservation";
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    _reservation = JsonConvert.DeserializeObject<List<Reservation>>(data);
                    List<Reservation> removeR = new();
                    foreach (Reservation r in _reservation)
                    {

                        if (r.NameUser != email)
                        {
                            removeR.Add(r);
                        }
                    }
                    foreach (Reservation r in removeR)
                    {
                        _reservation.Remove(r);
                    }
                    if (_reservation.Count == 0)
                        return NoContent();
                    else
                        return Ok(_reservation);
                }
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
        return BadRequest();
    }

    [HttpDelete]
    public async Task<ActionResult> Delete([FromBody] Reservation reservation)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string Url = "http://localhost:5246/Datapi/resData/Reservation";

                string requestData = JsonConvert.SerializeObject(reservation);

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

                    using (HttpClient client2 = new HttpClient())
                    {
                        string Url2 = "http://localhost:5033/Labapi/Lab/RemoveReservation";

                        string Data = JsonConvert.SerializeObject(reservation);

                        HttpRequestMessage Datarequest = new HttpRequestMessage
                        {
                            Method = HttpMethod.Delete,
                            RequestUri = new Uri(Url2),
                            Content = new StringContent(Data, Encoding.UTF8, "application/json")
                        };

                        HttpResponseMessage response2 = await client.SendAsync(Datarequest);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseData2 = await response2.Content.ReadAsStringAsync();
                            return Ok();
                        }
                        else
                        {
                            return Conflict();
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
    }

}