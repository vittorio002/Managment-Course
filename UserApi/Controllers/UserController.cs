using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("Userapi/[controller]")]
    public class UserController : ControllerBase
    {
        private static List<User> _users = new List<User>();

        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            try
            {
                using (HttpClient request = new HttpClient())
                {
                    string apiUrl = "http://localhost:5246/Datapi/UserData/User";
                    HttpResponseMessage response = request.GetAsync(apiUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        _users = JsonConvert.DeserializeObject<List<User>>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return Ok(_users);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] User user)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string Url = "http://localhost:5246/Datapi/UserData/User/GetUser";
                    StringContent content = new StringContent(JsonConvert.SerializeObject(user.Email), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(Url, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();

                        using (HttpClient request = new HttpClient())
                        {
                            string Url2 = "http://localhost:5246/Datapi/UserData/User/Add";

                            string requestData = JsonConvert.SerializeObject(user);

                            StringContent content2 = new StringContent(requestData, Encoding.UTF8, "application/json");

                            HttpResponseMessage response2 = await client.PostAsync(Url2, content2);

                            if (response.IsSuccessStatusCode)
                            {
                                string responseData2 = await response.Content.ReadAsStringAsync();
                            }
                            else
                            {
                                return BadRequest();
                            }
                        }
                        var resourceUrl = Request.Path.ToString() + '/' + user.Email;
                        return Created(resourceUrl, user);
                    }
                    else
                    {
                        return Conflict("Cannot create the User ... exists");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] User user)
        {
            try
            {
                User? existingUser = _users.Find(x => x.Email == user.Email);
                if (existingUser == null)
                {
                    return BadRequest("Cannot update … term.");
                }
                else
                {
                    if (user.Name != "")
                    {
                        existingUser.Name = user.Name;
                    }
                    existingUser.role = user.role;
                    using (HttpClient client = new HttpClient())
                    {
                        string Url = "http://localhost:5246/Datapi/UserData/User";

                        string requestData = JsonConvert.SerializeObject(existingUser);

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
        public async Task<ActionResult> Delete([FromBody] string Email)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string Url = "http://localhost:5246/Datapi/UserData/User";

                    string requestData = JsonConvert.SerializeObject(Email);

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
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return NoContent();
        }
    }
}