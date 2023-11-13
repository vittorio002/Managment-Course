public class Reservation{
    public string NameUser {get;set;}
    public string NamePc {get;set;}
    public int Hour {get;set;}
    public string Date {get;set;}
    public Reservation(string nameUser, string namePc, int hour, string date){
        NameUser = nameUser;
        NamePc = namePc;
        Hour = hour;
        Date = date;
    }
}