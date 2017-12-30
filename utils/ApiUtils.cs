using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace utils
{
    public class ApiUtils
    {

        public static void Send(RequestPay rp, int i)
        {
            var x = JsonConvert.SerializeObject(rp);

            using (var client = new HttpClient())
            {
                var httpContent = new HttpRequestMessage(HttpMethod.Post, $"http://localhost:809{i}/set");

                httpContent.Content = new StringContent(x, Encoding.UTF8, "application/json");
                var response = client.SendAsync(httpContent).Result.Content.ReadAsStringAsync().Result;
                //    Console.WriteLine("Send! response " + response);
            }
        }
    }

}
