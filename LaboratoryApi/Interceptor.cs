using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

//the interceptor where is defined he start first the run of method, for verify if the token is valid
public class TokenValidation : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
            if (!filterContext.HttpContext.Request.Headers.TryGetValue("Authentication", out var token) || !IsValidToken(token)) //verify if the token exist and if is valid
            {
                filterContext.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                return;


            }
            base.OnActionExecuting(filterContext);
    }

//verify if is valid
    private bool IsValidToken(string token)
    {
        //split the string to get the email
        string[] elements = token.Split(';');

        foreach (var element in elements)
        {
            string[] keyValue = element.Split(':');

            string key = keyValue[0].Trim();
            string value = keyValue[1].Trim();

            if (key.Equals("{\"email\""))
            {
                string pattern = "\"([^\"]+)\""; //transform json syntax to string

                Match match = Regex.Match(value, pattern);

                if (match.Success)
                {
                    string extractedValue = match.Groups[1].Value;


                    using (HttpClient client = new HttpClient())//call to get the user token
                    {
                        string Url = "http://localhost:5246/Datapi/UserData/User/TakeToken";
                        StringContent content = new StringContent(JsonConvert.SerializeObject(extractedValue), Encoding.UTF8, "application/json");
                        HttpResponseMessage response = client.PostAsync(Url, content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            string responseData = response.Content.ReadAsStringAsync().Result;
                            string[] t = responseData.Split("-");//divides the token by the milliseconds of creation or last user action
                            if (t[0] == token)
                            {
                                
                                //takes the current date and turns it into milliseconds
                                long newTicks = DateTime.Now.Ticks;
                                long newMilliseconds = newTicks / TimeSpan.TicksPerMillisecond;
                                long Takems = long.Parse(t[1]);
                                double minutesDifference = (newMilliseconds - Takems) / (1000.0 * 60);

                                string newToken = token + "-" + newMilliseconds;

                                if (minutesDifference <= 5)//token expires, 5 minutes
                                {
                                    using (HttpClient request = new HttpClient()) //called to update the token date
                                    {
                                        string StorageTokenUrl = "http://localhost:5246/Datapi/UserData/User/StorageToken";
                                        StringContent content2 = new StringContent(JsonConvert.SerializeObject(newToken), Encoding.UTF8, "application/json");
                                        HttpResponseMessage response2 = client.PostAsync(StorageTokenUrl, content2).Result;
                                        if (response2.IsSuccessStatusCode)
                                        {
                                            return true;//return true if is valid
                                        }
                                    }
                                }
                                else
                                {
                                    return false;//return false if is expired
                                }
                            }
                            else
                                return false;//return false if is note equal
                        }
                    }
                }

            }
        }
        return false;
    }
}