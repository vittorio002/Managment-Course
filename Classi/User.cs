using Newtonsoft.Json;

namespace UserApi
{
    public class User
    {
        public string Email { get; private set; }
        public string Name { get; set; }
        [JsonProperty]
        public string Password { private get; set; }
        [JsonProperty]
        public string Nonce { private get; set; }
        public List<string> role { get; set; }
        public User(string email, string name, string password)
        {
            Email = email;
            Name = name;
            Password = password;
            role = new List<string>();
        }

        public string encrypting(string pwd, string NONCE)
        {
            return pwd += NONCE;
        }

        public bool Verify(string psw)
        {
            string cryptPsw = this.Password + this.Nonce;
            return psw == cryptPsw;
        }
        public string newNONCE()
        {
            string num = "";
            for (int i = 0; i < 10; i++)
            {
                Random IdR = new Random();
                int o = IdR.Next(0, 10);
                num += o;
            }
            this.Nonce = num;
            return num;
        }
        public string GetNonce(){
            return this.Nonce;
        }
    }
}