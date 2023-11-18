using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserApi;
using Requests;

namespace Data.Controllers
{
    [ApiController]
    [Route("Datapi/[controller]/User")]
    public class UserDataController : ControllerBase
    {
        private static List<User>? _users = new();

        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            try
            {
                Deserialize();

                return Ok(_users);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPost]
        [Route("Login")]
        public ActionResult<User> Login([FromBody] LoginRequest request)
        {
            try
            {
                Deserialize();
                User? user = _users.Find(x => x.Email == request.Email);
                bool verify = user.Verify(request.Password);
                Serialize();
                return verify == false ? BadRequest("email or password not valid") : Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPost]
        [Route("GetNonce")]
        public ActionResult<string> GetNonce([FromBody] string Email)
        {
            try
            {
                Deserialize();
                User? user = _users.Find(x => x.Email == Email);

                string n = user.newNONCE();
                Serialize();
                return n;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPost]
        [Route("GetUser")]
        public ActionResult<User> GetSingle([FromBody] string Email)
        {
            try
            {
                Deserialize();
                User? user = _users.Find(x => x.Email == Email);
                return user == null ? Ok() : Conflict("exist");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPost]
        [Route("StorageToken")]
        public ActionResult AssignToken([FromBody] string token)
        {
            try
            {
                string[] t = token.Split("-");
                User? user = JsonConvert.DeserializeObject<User>(t[0]);
                Deserialize();
                User u = _users.Find(us => us.Email == user.Email);
                u.setToken(token);
                Serialize();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost]
        [Route("TakeToken")]
        public ActionResult<string> TakeToken([FromBody] string email)
        {
            try
            {
                Deserialize();
                User user = _users.Find(u => u.Email == email);
                return user == null ? NotFound() : user.getToken();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("Add")]
        public ActionResult Post([FromBody] User user)
        {
            try
            {

                Deserialize();

                _users.Add(user);

                Serialize();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut]
        public ActionResult Put([FromBody] User user)
        {
            try
            {
                Deserialize();

                User? u = _users.Find(la => la.Email == user.Email);
                u.Name = user.Name;
                u.role = user.role;

                Serialize();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpDelete]
        public ActionResult Delete([FromBody] string email)
        {
            try
            {
                Deserialize();

                User? u = _users.Find(user => user.Email == email);
                _users.Remove(u);

                Serialize();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        private void Deserialize()
        {
            try
            {
                string json = RunJson.ReadUserFile();
                _users = JsonConvert.DeserializeObject<List<User>>(json);
            }
            catch (Exception ex)
            {
                BadRequest(ex);
            }
        }
        private void Serialize()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_users);
                RunJson.WriteUserFile(json);
            }
            catch (Exception ex)
            {
                BadRequest(ex);
            }
        }
    }
}