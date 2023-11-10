using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserApi;
using LaboratoryApi;

namespace Data.Controllers
{
    [ApiController]
    [Route("Datapi/[controller]/Lab")]
    public class LabDataController : ControllerBase
    {
        private static List<Lab>? _labs = new();
        
        [HttpGet]
        public ActionResult<List<Lab>> Get()
        {
            try{
            Deserialize();
            return Ok(_labs);
            }
            catch(Exception e){
                return BadRequest(e);
            }
        }
        
        [HttpPost]
        public ActionResult Post([FromBody] Lab lab)
        {
            try{
            Deserialize();
            _labs.Add(lab);
            Serialize();

            return Ok();
            }
            catch(Exception e){
                return BadRequest(e);
            }
        }
        
        [HttpPut]
        public ActionResult Put([FromBody] Lab lab)
        {
            try{
            Deserialize();

            Lab? l =_labs.Find(la => la.Name  == lab.Name);
            _labs.Remove(l);
            _labs.Add(lab);
            Serialize();

            return Ok();
            }
            catch(Exception e){
                return BadRequest(e);
            }
        }
        
        [HttpDelete]
        public ActionResult Delete([FromBody]string name)
        {
            try{
            Deserialize();

            Lab? l = _labs.Find(lab => lab.Name == name);
            _labs.Remove(l);

            Serialize();

            return Ok();
            }
            catch(Exception e){
                return BadRequest(e);
            }
        }
        private void Deserialize(){
            string json = RunJson.ReadLabFile();
            _labs = JsonConvert.DeserializeObject<List<Lab>>(json);
        }       
        private void Serialize(){
            string json = JsonConvert.SerializeObject(_labs);
            RunJson.WriteLabFile(json);
        }
    }
}