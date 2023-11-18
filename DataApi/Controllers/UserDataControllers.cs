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
        private static List<User>? _users = new(); //users list

        //return all users
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

        //take the email and password(whit the nonce), and verify if the credential are true
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

        //generate nonce inside the user who is logging
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

        //verify if the user alredy exist
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

        //take the token and set in a property of user
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

        //serch the user and take the token
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

        //add one user
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

        //modify one specific user
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

        //delete one user
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

        //call the method for read the json and deserialize in the Lab list 
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

        //serialize the list in string and send to metod for whrite the json
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