
using System;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using utils;
using Newtonsoft.Json;

public class MiniServer
{
    public void Stop()
    {
        runs = false;
    }
    private bool runs;
    public void Listen(string url, Queue<string> queue, Network net)
    {
        runs = true;
        var listener = new HttpListener();
        listener.Prefixes.Add(url);
        listener.Start();
        Task.Run(() =>
        {
            while (runs)
            {
                HttpListenerContext context = listener.GetContext();
                try
                {
                    if (context.Request.Url.PathAndQuery.EndsWith("/set"))
                    {
                        queue.Enqueue(new StreamReader(context.Request.InputStream).ReadToEnd());
                        //Adding permanent http response headers
                        var b = Encoding.UTF8.GetBytes("Thanks");
                        context.Response.OutputStream.Write(b, 0, b.Length);
                        Console.WriteLine("Ask Payment.");
                    }
                    else
                    {
                        var pub = new StreamReader(context.Request.InputStream).ReadToEnd();
                        var coin=net.all[pub];
                        var b = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(coin));
                        context.Response.OutputStream.Write(b, 0, b.Length);
                        Console.WriteLine("Sent Coin Info");
                    }
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
                context.Response.OutputStream.Flush();
                context.Response.OutputStream.Close();
            }
        });
    }
}