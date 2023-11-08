namespace LaboratoryApi
{
    public class RunJson
    {


        public string ReadUserFile() // Read the file and write in the Dictionaries
        {
            try
            {
            string json = File.ReadAllText("../Json/user.json");

            return json;

            }
            catch(Exception ex)
            {
                Console.WriteLine("Errore durante la lettura dei file: " + ex.Message);
                return null;
            }
        }

        public string ReadLabFile() // Read the file and write in the Dictionaries
        {
            try
            {
            string json = File.ReadAllText("../Json/lab.json");

            return json;

            }
            catch(Exception ex)
            {
                Console.WriteLine("Errore durante la lettura dei file: " + ex.Message);
                return null;
            }
        }

        public void WriteUserFile(string json) // Write the file with the Sictionaries
        {
            try
            {
            File.WriteAllText("../Json/user.json", json);

            }
            catch(Exception ex)
            {
                Console.WriteLine("Errore durante la scrittura dei file: " + ex.Message);
            }
        }
        public void WriteLabFile(string json) // Write the file with the Sictionaries
        {
            try
            {

            File.WriteAllText("../Json/lab.json", json);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Errore durante la scrittura dei file: " + ex.Message);
            }
        }
    }
}