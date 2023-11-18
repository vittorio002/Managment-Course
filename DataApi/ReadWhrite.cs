namespace Data
{
    public class RunJson
    {
        //read the json and return a string
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

        //read the json and return a string
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

        //read the json and return a string
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

        //take a string and whrite in the json
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

        //take a string and whrite in the json
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

        //take a string and whrite in the json
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