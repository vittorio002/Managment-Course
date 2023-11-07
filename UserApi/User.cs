namespace UserApi
{
    public class User
    {
        public string Email { get; private set; }
        public string Name { get; set; }
        private string Password { get; set; }
        private Random NONCE {get;set;}
        public int timeWork {get;set;}
        public List<DateTime> reservation {get;set;}
        public bool admin {get;set;}
        public bool labManager {get;set;}
        public User(string email, string name, string password)
        {
            Email = email;
            Name = name;
            Password = password;
            NONCE = new Random();
            timeWork = 0;
            admin = false;
            labManager = false;
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