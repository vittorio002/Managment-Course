namespace LaboratoryApi
{
    public class Computer{
        public string Name{get;set;}
        public int id {get;set;}
        public List<string> program {get;set;}
        public Computer(string name){
            Name = name;
        }
    }
}