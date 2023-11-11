namespace LaboratoryApi
{
    public class Lab
    {
        public string Name {get;set;}
        public List<Computer> computers {get;set;}
        public Lab(string name){
            Name = name;
            computers = new List<Computer>();
        }
    }
}