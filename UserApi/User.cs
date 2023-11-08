using Reservations;
namespace UserApi
{
    public class User
    {
        public string Email { get; private set; }
        public string Name { get; set; }
        private string Password { get; set; }
        private Random NONCE {get;set;}
        public int timeWork {get;set;}
        public List<Reservation> Reservations {get;set;}
        public List<string> role {get;set;}
        public User(string email, string name, string password)
        {
            Email = email;
            Name = name;
            Password = password;
            NONCE = new Random();
            timeWork = 0;
            this.Reservations = new List<Reservation>();
            role = new List<string>();
        }

        public Random GetNONCE(){
            return this.NONCE;
        }

        public string encrypting(string pwd){
            return pwd+=NONCE;
        }

        public bool Equals(string psw){
            return psw == encrypting(this.Password);
        }
    }
}