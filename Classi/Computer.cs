//using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;

namespace LaboratoryApi
{
    public class Computer{
        public string Name{get;set;}
        public string Id {get;set;}
        public List<string> program {get;set;}
        public Computer(string name){
            Name = name;
            Id = randomId();
            program = new List<string>();
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