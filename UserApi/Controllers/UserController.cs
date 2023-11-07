using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private static List<User> _users = new List<User>();

        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            return Ok(_users);
        }
        [HttpGet]
        [Route("{Email}")]
        public ActionResult<User> Get(string Email, string Password)
        {
            User? user=_users.Find(x=> x.Email == Email);
            Random Nonce = user.GetNONCE();
            Password+=Nonce;
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
            var user=_users.Find(x=> x.Email == Email);
            Random Nonce = user.GetNONCE();
            Password+=Nonce;
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
        public void DeserializeFile(string json) // Read the file and write in the Dictionaries
        {
            try
            {
                _users = JsonConvert.DeserializeObject<List<User>>(json);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore durante la lettura dei file: " + ex.Message);
            }
        }

        public string SerializeFile() // Write the file with the Sictionaries
        {
            try
            {
                string json = JsonConvert.SerializeObject(_users);

                return (json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore durante la scrittura dei file: " + ex.Message);
                return null;
            }
        }
    }
}