using Newtonsoft.Json;

namespace UserApi
{
    public class User
    {
        public string Email { get; private set; }
        public string Name { get; set; }
        [JsonProperty]
        public string Password { private get; set; }
        public List<string> role {get;set;}
        public User(string email, string name, string password)
        {
            Email = email;
            Name = name;
            Password = password;
            role = new List<string>();
        }

        public string encrypting(string pwd, string NONCE){
            return pwd+=NONCE;
        }

        public bool Equals(string psw, string NONCE){
            return psw == encrypting(this.Password, NONCE);
        }
    }
}