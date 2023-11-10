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
        private string? Nonce;

        [HttpGet]
        public ActionResult<List<User>> Get()
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
            return Ok(_users);
        }
        [HttpGet]
        [Route("{Email}")]
        public ActionResult<User> Get(string Email)
        {
            User? user = _users.Find(x => x.Email == Email);
            //this.Nonce = newNONCE();
            //Password += Nonce;
            //bool verify = user.Equals(Password);
            return user == null /*|| verify == false*/ ? NotFound() : Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult> Post(User user)
        {
            User? existingUser = _users.Find(x => x.Email == user.Email);
            if (existingUser != null)
            {
                return Conflict("Cannot create the User … exists.");
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    string Url = "http://localhost:5246/Datapi/UserData/User";

                    string requestData = JsonConvert.SerializeObject(user);

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
                var resourceUrl = Request.Path.ToString() + '/' + user.Email;
                return Created(resourceUrl, user);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Put(User user)
        {
            User? existingUser = _users.Find(x => x.Email == user.Email);
            if (existingUser == null)
            {
                return BadRequest("Cannot update … term.");
            }
            else
            {
                 using (HttpClient client = new HttpClient())
                {
                    string Url = "http://localhost:5246/Datapi/UserData/User";

                    string requestData = JsonConvert.SerializeObject(user);

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
                //existingUser.Name = user.Name;
                return Ok();
            }
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(string Email)
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
                return NoContent();
            //}
        }
        private string newNONCE()
        {
            string Nonce = "";
            for (int i = 0; i < 10; i++)
            {
                Random IdR = new Random();
                int o = IdR.Next(0, 10);
                Nonce += o;
            }
            return Nonce;
        }
    }
}