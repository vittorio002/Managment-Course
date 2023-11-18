using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Requests;

[ApiController]
[Route("Userapi/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("GetNonce")]
    public async Task<ActionResult<string>> GetNonce([FromBody] string Email)
    {
        try
        {
            using (HttpClient request = new HttpClient())
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
    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            using (HttpClient UserRequest = new HttpClient())
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

                        using (HttpClient client = new HttpClient())
                        {
                            string Url = "http://localhost:5246/Datapi/UserData/User/StorageToken";
                            StringContent DataContent = new StringContent(JsonConvert.SerializeObject(d), Encoding.UTF8, "application/json");
                            HttpResponseMessage response2 = await UserRequest.PostAsync(Url, DataContent);
                            if (response.IsSuccessStatusCode)
                            {
                                string Data = await response.Content.ReadAsStringAsync();
                                //string encryptedData = Encrypt(data);
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
    /*private static string Encrypt(string data)
   {
       using (Aes aesAlg = Aes.Create())
       {
           string key = "token";
           aesAlg.Key = Encoding.UTF8.GetBytes(key);
           aesAlg.IV = new byte[16];

           ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

           using (MemoryStream msEncrypt = new MemoryStream())
           {
               using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
               {
                   using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                   {
                       swEncrypt.Write(data);
                   }
               }
               return Convert.ToBase64String(msEncrypt.ToArray());
           }
       }
   }*/

}