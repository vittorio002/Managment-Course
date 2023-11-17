namespace Data
{
    public class RunJson
    {
        public static string ReadUserFile()
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

        public static string ReadLabFile()
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
        public static string ReadResFile()
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
        public static void WriteUserFile(string json)
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
        public static void WriteLabFile(string json)
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
         public static void WriteResFile(string json)
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