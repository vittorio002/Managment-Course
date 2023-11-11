using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Requests;
using UserApi;

[ApiController]
[Route("Userapi/[controller]")]
public class AuthController : ControllerBase
{
    private string Nonce;
    [HttpPost("GetNonce")]
    public ActionResult<string> GetNonce([FromBody] string email)
    {
         using (HttpClient request = new HttpClient())
            {
                string apiUrl = "http://localhost:5246/Datapi/UserData/User/GetNonce";
                var jsonContent = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");
                HttpResponseMessage response = request.PostAsync(apiUrl, jsonContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    return Ok(data);
                }
            }
        return BadRequest();
    }
    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        using (HttpClient UserRequest = new HttpClient())
        {
            var apiUrl = "http://localhost:5246/Datapi/UserData/User/GetUser";

            var requestData = new
            {
                Email = request.Email,
                Password = request.Password,
                Nonce = Nonce
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            HttpResponseMessage response = UserRequest.PostAsync(apiUrl, jsonContent).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                User? user = JsonConvert.DeserializeObject<User>(data);

                if (user == null)
                {
                    NotFound();
                }
                else
                {
                    return Ok(user);
                }
            }
            else
            {
                BadRequest("response error");
            }
        }
        return BadRequest("url error");
    }
}