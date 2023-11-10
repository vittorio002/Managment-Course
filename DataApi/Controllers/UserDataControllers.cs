using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserApi;

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
            try{
            Deserialize();

            return Ok(_users);
            }
            catch(Exception e){
                return BadRequest(e);
            }
        }
        [HttpPost]
        public ActionResult Post([FromBody] User user)
        {
            try{          

            Deserialize();

            _users.Add(user);
            
            Serialize();
            
            return Ok();
            }
            catch(Exception e){
                return BadRequest(e);
            }
        }
        [HttpPut]
        public ActionResult Put([FromBody] User user)
        {
            try{
            Deserialize();

            User? u =_users.Find(la => la.Email  == user.Email);
            _users.Remove(u);
            _users.Add(user);

            Serialize();

            return Ok();
            }
            catch(Exception e){
                return BadRequest(e);
            }
        }
        [HttpDelete]
        public ActionResult Delete([FromBody] string email)
        {
            try{
            Deserialize();

            User? u = _users.Find(user => user.Email == email);
            _users.Remove(u);

            Serialize();

            return Ok();
            }
            catch(Exception e){
                return BadRequest(e);
            }
        }
        private void Deserialize(){
            string json = RunJson.ReadUserFile();
            _users = JsonConvert.DeserializeObject<List<User>>(json);
        }       
        private void Serialize(){
            string json = JsonConvert.SerializeObject(_users);
            RunJson.WriteUserFile(json);
        }
    }
}