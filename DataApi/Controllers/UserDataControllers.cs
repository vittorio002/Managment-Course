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
        public ActionResult<List<User>> Get()
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
        [Route("GetUser")]
        public ActionResult<User> Get([FromBody] LoginRequest getUserParameters)
        {
            try
            {
                Deserialize();
                User? user = _users.Find(x => x.Email == getUserParameters.Email);
                bool verify = user.Verify(getUserParameters.Password);
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
        public ActionResult<string> Get([FromBody] string Email)
        {
            try
            {
                Deserialize();
                User? user = _users.Find(x => x.Email == Email);
                
                string n = user.newNONCE();
                //_users.Add(user);
                Serialize();
                return n;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPost]
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
                _users.Remove(u);
                _users.Add(user);

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
            string json = RunJson.ReadUserFile();
            _users = JsonConvert.DeserializeObject<List<User>>(json);
        }
        private void Serialize()
        {
            string json = JsonConvert.SerializeObject(_users);
            RunJson.WriteUserFile(json);
        }
    }
}