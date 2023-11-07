using LaboratoryApi;

namespace Reservations{
    public class Reservation{
        public DateOnly Date {get;set;}
        public int Hour {get;set;}
        public Computer Pc {get;set;}
        public Reservation(DateOnly date, int hour, Computer computer){
            Date = date;
            Hour = hour;
            Pc = computer;
        }
    }
}