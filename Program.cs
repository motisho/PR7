using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace PR7
{
    public class Program
    {
        static void Main(string[] args)
        {
            Cookie token = SignIn("user", "user");
            GetContent(token);
            Console.Read();
        }
        public static Cookie SignIn(string Login, string Password)
        {
            Cookie token = null;
            string url = "http://news.permaviat.ru/ajax/login.php";
            Debug.WriteLine($"Выполняю запрос: {url}");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();
            string postData = $"login={Login}&password={Password}";
            byte[] Data = Encoding.ASCII.GetBytes(postData);
            request.ContentLength = Data.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(Data, 0, Data.Length);
            }
            using (HttpWebResponse Response = (HttpWebResponse)request.GetResponse())
            {
                Debug.WriteLine($"Статус выполенения: {Response.StatusCode}");
                string ResponseFromServer = new StreamReader(Response.GetResponseStream()).ReadToEnd();
                Console.WriteLine(ResponseFromServer);
                token = Response.Cookies["token"];
            }
            return token;
        }
        public static void GetContent(Cookie Token)
        {
            string url = "http://news.permaviat.ru/main";
            Debug.WriteLine($"Выполняем зпарос: {url}");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(Token);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Debug.WriteLine($"Статус выполнения: {response.StatusCode}");
            string responseFromServer = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Console.WriteLine(responseFromServer);
        }
    }
}
