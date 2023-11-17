using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Data.Controllers
{
    [ApiController]
    [Route("Datapi/[controller]/Reservation")]
    public class ResDataController : ControllerBase
    {
        private static List<Reservation> _reservation = new List<Reservation>();
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