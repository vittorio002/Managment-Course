namespace Requests;

public class LoginRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class ReserveRequest
{
    public string? Date {get;set;}
    public int Hour {get;set;}
    public ReserveRequest(string date, int hour){
        Date=date;
        Hour=hour;
    }
}