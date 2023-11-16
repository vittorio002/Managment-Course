using Requests;

namespace LaboratoryApi
{
    public class Computer{
        public string Name{get;set;}
        public string Id {get;set;}
        public bool Status {get;set;}
        public List<string> program {get;set;}
        public List<ReserveRequest> Reserve {get;set;}
        public Computer(string name){
            Name = name;
            Id = randomId();
            Status = true;
            program = new List<string>();
            Reserve = new List<ReserveRequest>();
        }

        public string randomId(){
            string id = "";
            for(int i = 0; i < 10; i++){
            Random IdR = new Random();
            int o = IdR.Next(0, 10);
            id += o;
            }
            return id;
        }
    }
}