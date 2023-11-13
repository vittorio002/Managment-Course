using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Requests;

[ApiController]
[Route("Userapi/[controller]")]
public class AuthController : ControllerBase
{
    //private string Nonce;
    [HttpPost("GetNonce")]
    public async Task<ActionResult<string>> GetNonce([FromBody] string Email)
    {
         using (HttpClient request = new HttpClient())
            {
                string apiUrl = "http://localhost:5246/Datapi/UserData/User/GetNonce";
                StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(Email), Encoding.UTF8, "application/json");
                HttpResponseMessage response = request.PostAsync(apiUrl, jsonContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    return Content(data);
                }
            }
        return NotFound();
    }
    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        try{
            using (HttpClient UserRequest = new HttpClient())
            {
                string apiUrl = "http://localhost:5246/Datapi/UserData/User/Login";
                /*var requestData = new
                {
                    Email = request.Email,
                    Password = request.Password,
                    //Nonce = Nonce
                };*/
                StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await UserRequest.PostAsync(apiUrl, jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    //User? user = JsonConvert.DeserializeObject<User>(data);

                    if (data == null)
                    {
                        NotFound();
                    }
                    else
                    {
                        return Content(data);
                    }
                }
                else
                {
                    BadRequest("response error");
                }
            }
        }
        catch(Exception ex){
            return StatusCode(500, "internal error "+ex);
        }
        return StatusCode(500, "internal error ");
    }
}