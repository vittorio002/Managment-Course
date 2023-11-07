namespace Json
{
    public class RunJson
    {


        public void ReadUserFile() // Read the file and write in the Dictionaries
        {
            try
            {
            string json = File.ReadAllText("../Json/user.json");

            company.DeserializeFile(json);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Errore durante la lettura dei file: " + ex.Message);
            }
        }

        public void ReadLabFile() // Read the file and write in the Dictionaries
        {
            try
            {
            string json = File.ReadAllText("../Json/lab.json");

            company.DeserializeFile(json);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Errore durante la lettura dei file: " + ex.Message);
            }
        }

        public void WriteUserFile() // Write the file with the Sictionaries
        {
            try
            {
            string json = company.SerializeFile();
            File.WriteAllText("../Json/user.json", json);

            }
            catch(Exception ex)
            {
                Console.WriteLine("Errore durante la scrittura dei file: " + ex.Message);
            }
        }
        public void WriteLabFile() // Write the file with the Sictionaries
        {
            try
            {
            string json = company.SerializeFile();

            File.WriteAllText("../Json/lab.json", json);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Errore durante la scrittura dei file: " + ex.Message);
            }
        }
    }
}