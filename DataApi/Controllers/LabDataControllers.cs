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
        public ActionResult Post(Lab lab)
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
        
        [HttpPut("{name}")]
        public ActionResult Put(string name, Lab lab)
        {
            try{
            Deserialize();

            Lab? l =_labs.Find(la => la.Name  == name);
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
        [Route("{name}")]
        public ActionResult Delete(string name)
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