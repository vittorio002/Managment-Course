using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Requests;

[ApiController]
[Route("Userapi/[controller]")]
public class AuthController : ControllerBase
{
    //create and take the token from user
    [HttpPost("GetNonce")]
    public async Task<ActionResult<string>> GetNonce([FromBody] string Email)
    {
        try
        {
            using (HttpClient request = new HttpClient()) //call to get the nonce from the user in the date service
            {
                string apiUrl = "http://localhost:5246/Datapi/UserData/User/GetNonce";
                StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(Email), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await request.PostAsync(apiUrl, jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    return Content(data);
                }
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
        return NotFound();
    }

    //take the request(email and password+nonce) and verify if in the user the password is the same
    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            using (HttpClient UserRequest = new HttpClient())//call to data service for verify if in the object user the password is the same
            {
                string apiUrl = "http://localhost:5246/Datapi/UserData/User/Login";

                StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await UserRequest.PostAsync(apiUrl, jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();

                    if (data == null)
                    {
                        NotFound();
                    }
                    else
                    {
                        string d = data;
                        long tick = DateTime.Now.Ticks;
                        long ms = tick / TimeSpan.TicksPerMillisecond;
                        d += "-" + ms;

                        using (HttpClient client = new HttpClient())//call to storage the token and the date of creation in milliseconds
                        {
                            string Url = "http://localhost:5246/Datapi/UserData/User/StorageToken";
                            StringContent DataContent = new StringContent(JsonConvert.SerializeObject(d), Encoding.UTF8, "application/json");
                            HttpResponseMessage response2 = await UserRequest.PostAsync(Url, DataContent);
                            if (response.IsSuccessStatusCode)
                            {
                                string Data = await response.Content.ReadAsStringAsync();
                                return Content(data);
                            }
                        }
                    }
                }
                else
                {
                    BadRequest("response error");
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "internal error " + ex);
        }
        return StatusCode(500, "internal error ");
    }
}