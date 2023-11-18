using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Data.Controllers
{
    [ApiController]
    [Route("Datapi/[controller]/Reservation")]
    public class ResDataController : ControllerBase
    {
        private static List<Reservation> _reservation = new List<Reservation>(); //Reservation List

        //return all reservation
        [HttpGet]
        public ActionResult<List<Reservation>> Get()
        {
            try
            {
                Deserialize();
                return Ok(_reservation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //add one reservation
        [HttpPost]
        public ActionResult Post([FromBody] Reservation reservation)
        {
            try
            {
                Deserialize();
                _reservation.Add(reservation);
                Serialize();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //take one reservation in input and delete the corresponding
        [HttpDelete]
        public ActionResult Delete([FromBody] Reservation reservation)
        {
            try
            {
                Deserialize();
                Reservation res = _reservation.Find(r => r.NamePc == reservation.NamePc && r.NameUser == reservation.NameUser && r.Date == reservation.Date && r.Hour == reservation.Hour);
                _reservation.Remove(res);
                Serialize();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //call the method for read the json and deserialize in the Lab list 
        private void Deserialize()
        {
            try
            {
            string json = RunJson.ReadResFile();
            _reservation = JsonConvert.DeserializeObject<List<Reservation>>(json);
            }
            catch(Exception ex){
                BadRequest(ex);
            }
        }

        //serialize the list in string and send to metod for whrite the json
        private void Serialize()
        {
            try{
            string json = JsonConvert.SerializeObject(_reservation);
            RunJson.WriteResFile(json);
            }
            catch(Exception ex){
                BadRequest(ex);
            }
        }
    }
}