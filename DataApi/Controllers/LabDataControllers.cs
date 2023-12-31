using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Requests;
using LaboratoryApi;

namespace Data.Controllers
{
    [ApiController]
    [Route("Datapi/[controller]/Lab")]
    public class LabDataController : ControllerBase
    {
        private static List<Lab> _labs = new(); //Laboratory List
        
        //return all Labs
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
        
        //take Object Lab and modifay the lab in the json 
        [HttpPut]
        public ActionResult Put([FromBody] Lab lab)
        {
            try{
            Deserialize();

            Lab? l =_labs.Find(la => la.Name  == lab.Name);
            l.computers = lab.computers;
            Serialize();

            return Ok();
            }
            catch(Exception e){
                return BadRequest(e);
            }
        }

        //call the method for read the json and deserialize in the Lab list       
        private void Deserialize(){
            try
            {
            string json = RunJson.ReadLabFile();
            _labs = JsonConvert.DeserializeObject<List<Lab>>(json);
            }
            catch(Exception ex){
                BadRequest(ex);
            }
        }       

        //serialize the list in string and send to metod for whrite the json
        private void Serialize(){
            try{
            string json = JsonConvert.SerializeObject(_labs);
            RunJson.WriteLabFile(json);
            }
            catch(Exception ex){
                BadRequest(ex);
            }
        }
    }
}