namespace Data
{
    public class RunJson
    {
        public static string ReadUserFile() // Read the file and write in the Dictionaries
        {
            try
            {
            string json = File.ReadAllText("user.json");

            return json;

            }
            catch(Exception ex)
            {
                Console.WriteLine("Errore durante la lettura dei file: " + ex.Message);
                return null;
            }
        }

        public static string ReadLabFile() // Read the file and write in the Dictionaries
        {
            try
            {
            string json = File.ReadAllText("lab.json");

            return json;

            }
            catch(Exception ex)
            {
                Console.WriteLine("Errore durante la lettura dei file: " + ex.Message);
                return null;
            }
        }
        public static string ReadResFile() // Read the file and write in the Dictionaries
        {
            try
            {
            string json = File.ReadAllText("reservation.json");

            return json;

            }
            catch(Exception ex)
            {
                Console.WriteLine("Errore durante la lettura dei file: " + ex.Message);
                return null;
            }
        }
        public static void WriteUserFile(string json) // Write the file with the Sictionaries
        {
            try
            {
            File.WriteAllText("user.json", json);

            }
            catch(Exception ex)
            {
                Console.WriteLine("Errore durante la scrittura dei file: " + ex.Message);
            }
        }
        public static void WriteLabFile(string json) // Write the file with the Sictionaries
        {
            try
            {

            File.WriteAllText("lab.json", json);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Errore durante la scrittura dei file: " + ex.Message);
            }
        }
         public static void WriteResFile(string json) // Write the file with the Sictionaries
        {
            try
            {

            File.WriteAllText("reservation.json", json);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Errore durante la scrittura dei file: " + ex.Message);
            }
        }
    }
}