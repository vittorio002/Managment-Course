using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Data.Controllers
{
    [ApiController]
    [Route("Datapi/[controller]/Reservation")]
    public class ResDataController : ControllerBase
    {
        private static List<Reservation> _reservation = new List<Reservation>{new Reservation("triolovi@gmail.com","Pc1",12,"12/12/2023")};

        [HttpGet]
        public ActionResult<List<Reservation>> Get()
        {
            //Deserialize();
            Serialize();
            return Ok(_reservation);
        }
        [HttpPost]
        public ActionResult Post([FromBody] Reservation reservation)
        {
            Deserialize();
            _reservation.Add(reservation);
            Serialize();
            return Ok();
        }
        [HttpDelete]
        public ActionResult Delete([FromBody]Reservation reservation)
        {
            Deserialize();
            Reservation res = _reservation.Find(r => r.NamePc == reservation.NamePc && r.NameUser == reservation.NameUser && r.Date == reservation.Date && r.Hour == reservation.Hour);
            _reservation.Remove(res);
            Serialize();
            return Ok();
        }
         private void Deserialize(){
            string json = RunJson.ReadResFile();
            _reservation = JsonConvert.DeserializeObject<List<Reservation>>(json);
        }       
        private void Serialize(){
            string json = JsonConvert.SerializeObject(_reservation);
            RunJson.WriteResFile(json);
        }
    }
}