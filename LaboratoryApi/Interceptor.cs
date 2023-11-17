using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.HttpResults;

public class TokenValidation : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
            if (!filterContext.HttpContext.Request.Headers.TryGetValue("Authentication", out var token) || !IsValidToken(token))
            {
                filterContext.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                return;


            }
            base.OnActionExecuting(filterContext);
    }

    private bool IsValidToken(string token)
    {
        string[] elements = token.Split(';');

        foreach (var element in elements)
        {
            string[] keyValue = element.Split(':');

            string key = keyValue[0].Trim();
            string value = keyValue[1].Trim();

            if (key.Equals("{\"email\""))
            {
                string pattern = "\"([^\"]+)\"";

                Match match = Regex.Match(value, pattern);

                if (match.Success)
                {
                    string extractedValue = match.Groups[1].Value;


                    using (HttpClient client = new HttpClient())
                    {
                        string Url = "http://localhost:5246/Datapi/UserData/User/TakeToken";
                        StringContent content = new StringContent(JsonConvert.SerializeObject(extractedValue), Encoding.UTF8, "application/json");
                        HttpResponseMessage response = client.PostAsync(Url, content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            string responseData = response.Content.ReadAsStringAsync().Result;
                            string[] t = responseData.Split("-");
                            if (t[0] == token)
                            {

                                long newTicks = DateTime.Now.Ticks;
                                long newMilliseconds = newTicks / TimeSpan.TicksPerMillisecond;
                                long Takems = long.Parse(t[1]);
                                double minutesDifference = (newMilliseconds - Takems) / (1000.0 * 60);

                                string newToken = token + "-" + newMilliseconds;

                                if (minutesDifference <= 5)
                                {
                                    using (HttpClient request = new HttpClient())
                                    {
                                        string StorageTokenUrl = "http://localhost:5246/Datapi/UserData/User/StorageToken";
                                        StringContent content2 = new StringContent(JsonConvert.SerializeObject(newToken), Encoding.UTF8, "application/json");
                                        HttpResponseMessage response2 = client.PostAsync(StorageTokenUrl, content2).Result;
                                        if (response2.IsSuccessStatusCode)
                                        {
                                            return true;
                                        }
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                                return false;
                        }
                    }
                }

            }
        }
        return false;
    }
    /*private static string Decrypt(string encryptedData)
    {
        using (Aes aesAlg = Aes.Create())
        {
            string key = "token";
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = new byte[16];

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedData)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }*/
}