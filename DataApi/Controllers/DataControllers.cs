using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LaboratoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private static List<Object> _labs = new();
        private static List<Object> _users = new();
        
        [HttpGet]
        [Route("{Lab}")]
        public ActionResult<List<Object>> GetLab()
        {
            return Ok(_labs);
        }
        [HttpGet]
        [Route("{User}")]
        public ActionResult<List<Object>> GetUser()
        {
            return Ok(_users);
        }
        [HttpPost]
        public ActionResult PostLab(Object lab)
        {
            _labs.Add(lab);
            return Ok();
        }
        [HttpPost]
        public ActionResult PostUser(Object user)
        {
            _users.Add(user);
            return Ok();
        }
        [HttpPut]
        public ActionResult PutLab(Object lab)
        {
            Object? l =_labs.Find(la => la  == lab);
            l = lab;
            return Ok();
        }
        public ActionResult PutUser(Object user)
        {
            Object? u =_users.Find(la => la  == user);
            u = user;
            return Ok();
        }
        [HttpDelete]
        [Route("{Lab}")]
        public ActionResult DeleteLab(Object lab)
        {
            _labs.Remove(lab);
            return Ok();
        }
        [HttpDelete]
        [Route("{User}")]
        public ActionResult DeleteUser(Object user)
        {
            _users.Remove(user);
            return Ok();
        }
    }
}