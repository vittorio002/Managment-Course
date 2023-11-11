namespace Requests;
public class GetUserParameters
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Nonce { get; set; }
    public GetUserParameters(string email, string password, string nonce){
        Email = email;
        Password=password;
        Nonce=nonce;
    }
}
public class LoginRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}