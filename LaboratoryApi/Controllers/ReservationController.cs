using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

[ApiController]
[Route("Reservation/[controller]")]
public class ReservationController : ControllerBase
{
    private static List<Reservation> _reservation = new();

    [HttpGet]
    public ActionResult<List<Reservation>> Get()
    {
        using (HttpClient request = new HttpClient())
            {
                string apiUrl = "http://localhost:5246/Datapi/resData/Reservation";
                HttpResponseMessage response = request.GetAsync(apiUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    _reservation = JsonConvert.DeserializeObject<List<Reservation>>(data);
                }
            }
        return Ok(_reservation);
    }
    [HttpPost]
    public async Task<ActionResult> Post([FromBody]Reservation reservation)
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
                        return Ok(reservation);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
    }
}