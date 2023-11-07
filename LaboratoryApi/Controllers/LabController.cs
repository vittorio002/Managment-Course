using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LaboratoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabController : ControllerBase
    {
        private static List<Lab> _labs = new List<Lab>();
        public void DeserializeFile(string json) // Read the file and write in the Dictionaries
        {
            try
            {
                _labs = JsonConvert.DeserializeObject<List<Lab>>(json);
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
                string json = JsonConvert.SerializeObject(_labs);

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