using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("Userapi/[controller]")]
    [TokenValidation]
    public class UserController : ControllerBase
    {
        private static List<User> _users = new List<User>();//users list

        //get all users
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            try
            {
                    HttpResponseMessage response = await GetAllUserRequest();
                     if (!response.IsSuccessStatusCode)
                    {
                        return BadRequest();
                    }

                        string data = response.Content.ReadAsStringAsync().Result;
                        _users = JsonConvert.DeserializeObject<List<User>>(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return Ok(_users);
        }

        //create and add new user
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] User user)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string Url = "http://localhost:5246/Datapi/UserData/User/GetUser";//call the data service to verify that the email does not already exist
                    StringContent content = new StringContent(JsonConvert.SerializeObject(user.Email), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(Url, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();

                        using (HttpClient request = new HttpClient())//call the data service for add the user in the json
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

        //modify the user
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] User user)
        {
            try
            {
                HttpResponseMessage response = await GetAllUserRequest();//take all users
                 if (!response.IsSuccessStatusCode)
                    {
                        return BadRequest();
                    }

                User? existingUser = _users.Find(x => x.Email == user.Email);
                if (existingUser == null) //verify if the user exist
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
                    using (HttpClient client = new HttpClient())//call to data service for update the json 
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

                        HttpResponseMessage response2 = await client.SendAsync(request);

                        if (!response2.IsSuccessStatusCode)
                        {
                            return BadRequest();
                        }
                        string responseData = await response2.Content.ReadAsStringAsync();
                    }
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //delete the user
        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] string Email)
        {
            try
            {
                using (HttpClient client = new HttpClient())//call to data service for delete the user from file
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

        private async Task<HttpResponseMessage> GetAllUserRequest()
        {
            using (HttpClient request = new HttpClient())
            {
                string apiUrl = "http://localhost:5246/Datapi/UserData/User";
                return request.GetAsync(apiUrl).Result;
            }
        }
    }
}