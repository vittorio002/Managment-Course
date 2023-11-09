using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("Userapi/[controller]")]
    public class UserController : ControllerBase
    {
        private static List<User> _users = new List<User>();
        private string? Nonce;

        [HttpGet("User")]
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
        [Route("User/{Email}")]
        public ActionResult<User> Get(string Email, string Password)
        {
            User? user = _users.Find(x => x.Email == Email);
            this.Nonce = newNONCE();
            Password += Nonce;
            bool verify = user.Equals(Password);
            return user == null || verify == false ? NotFound() : Ok(user);
        }
        [HttpPost]
        public ActionResult Post(User user)
        {
            User? existingUser = _users.Find(x => x.Email == user.Email);
            if (existingUser != null)
            {
                return Conflict("Cannot create the User … exists.");
            }
            else
            {
                _users.Add(user);
                var resourceUrl = Request.Path.ToString() + '/' + user.Email;
                return Created(resourceUrl, user);
            }
        }
        [HttpPut]
        public ActionResult Put(User user)
        {
            User? existingUser = _users.Find(x => x.Email == user.Email);
            if (existingUser == null)
            {
                return BadRequest("Cannot update … term.");
            }
            else
            {
                existingUser.Name = user.Name;
                return Ok();
            }
        }
        [HttpDelete]
        [Route("{Email}")]
        public ActionResult Delete(string Email, string Password, string CONFIRM)
        {
            string Confirm = CONFIRM;
            var user = _users.Find(x => x.Email == Email);
            this.Nonce = newNONCE();
            Password += Nonce;
            bool verify = user.Equals(Password);
            if
            ((user == null || verify == false) && Confirm != "CONFIRM")
            {
                return NotFound();
            }
            else
            {
                _users.Remove(user);
                return NoContent();
            }
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