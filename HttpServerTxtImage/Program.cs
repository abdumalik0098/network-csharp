using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace HttpServerTxtImage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8001/");

            listener.Start();

            Console.WriteLine("Listening on http://localhost:8001/");

            while (true)
            {
                HttpListenerContext ctx = listener.GetContext();
                HttpListenerRequest req = ctx.Request;

                string path = ctx.Request.Url?.LocalPath;

                //string pattern = @"\w*image\w*";
               // Regex regex = new Regex(pattern);

                
                if (path.Contains("image"))
                {
                    sendImage(ctx);
                }
                else if (path.Contains("text"))
                {
                    textSend(ctx);
                }
                else
                {
                    indexPage(ctx);
                }
            }
            void indexPage(HttpListenerContext ctx)
            {
                HttpListenerResponse resp = ctx.Response;
                //resp.Headers.Set("Content-Type", "text/plain");

                

               // ctx.Response.StatusCode = (int)HttpStatusCode.NotFound;
                string txt = "hello";

                byte[] ebuf = System.Text.Encoding.UTF8.GetBytes(txt);
                resp.ContentLength64 = ebuf.Length;

                Stream ros = resp.OutputStream;
                ros.Write(ebuf, 0, ebuf.Length);
                ros.Close();
            }

            void textSend(HttpListenerContext ctx)
            {
                HttpListenerResponse resp = ctx.Response;
                resp.Headers.Set("Content-Type", "text/plain");
                HttpListenerRequest req = ctx.Request;
                Stream ros = resp.OutputStream;
                string txt = $"./{req.Url.LocalPath}";

                byte[] ebuf = Encoding.UTF8.GetBytes(File.ReadAllText(txt, Encoding.UTF8));
                resp.ContentLength64 = ebuf.Length;

                ros.Write(ebuf, 0, ebuf.Length);
            }

            void sendImage(HttpListenerContext ctx)
            {
                HttpListenerResponse resp = ctx.Response;
                resp.Headers.Set("Content-Type", "image/jpeg");

                HttpListenerRequest req = ctx.Request;
                byte[] buf = File.ReadAllBytes($"./{req.Url.LocalPath}");
                resp.ContentLength64 = buf.Length;

                Stream ros = resp.OutputStream;
                ros.Write(buf, 0, buf.Length);
            }
        }
    }
}
