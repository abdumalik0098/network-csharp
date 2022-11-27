using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HtttpListener
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HttpListener server = new HttpListener();
            server.Prefixes.Add("http://localhost:8002/");

            server.Start();

            Console.WriteLine("Listening...");

            while (true)
            {
                HttpListenerContext context = server.GetContext();
                HttpListenerResponse response = context.Response;
                HttpListenerRequest req = context.Request;
                //response.Headers.Set("Content-Type", "text/html; image/png");
               // response.Headers.Set("Content-Type", "image/jpeg");

                // string page = Directory.GetCurrentDirectory() + context.Request.Url.LocalPath;
                string page = Directory.GetCurrentDirectory() + "\\src\\" + context.Request.Url.LocalPath;
                
                if (req.HttpMethod == "POST")
                {
                    ShowRequestData(req);
                    //using System.Web and Add a Reference to System.Web
                   /* Dictionary<string, string> postParams = new Dictionary<string, string>();
                    string[] rawParams = rawData.Split('&');
                    foreach (string param in rawParams)
                    {
                        string[] kvPair = param.Split('=');
                        string key = kvPair[0];
                        string value = HttpUtility.UrlDecode(kvPair[1]);
                        postParams.Add(key, value);
                    }

                    //Usage
                    Console.WriteLine("Hello " + postParams["username"]);*/


                    /* System.IO.Stream str; String strmContents;
                     Int32 counter, strLen, strRead;
                     // Create a Stream object.
                     str = req.InputStream;
                     // Find number of bytes in stream.
                     strLen = Convert.ToInt32(str.Length);
                     // Create a byte array.
                     byte[] strArr = new byte[strLen];
                     // Read stream into byte array.
                     strRead = str.Read(strArr, 0, strLen);

                     // Convert byte array to a text string.
                     strmContents = "";
                     for (counter = 0; counter < strLen; counter++)
                     {
                         strmContents = strmContents + strArr[counter].ToString();
                     }*/
                    /*string url = req.Url.ToString();
                    string body = req.InputStream.ToString();
                    Console.WriteLine($"{url}\n" +
                        $"Body: {body}");*/
                    // Console.WriteLine($"{strmContents}");
                    if (req.Url.AbsolutePath == "/list")
                    {
                        string listpage = Directory.GetCurrentDirectory() + "\\src\\list.html";

                        using (FileStream fs = File.OpenRead(listpage))
                        {
                            // выделяем массив для считывания данных из файла
                            byte[] buffer = new byte[fs.Length];
                            // считываем данные
                            await fs.ReadAsync(buffer, 0, buffer.Length);
                            response.ContentLength64 = buffer.Length;
                            Stream st = response.OutputStream;
                            st.Write(buffer, 0, buffer.Length);
                        }

                        System.IO.Stream body = req.InputStream;
                        System.Text.Encoding encoding = req.ContentEncoding;
                        System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);

                    }
                }

                if (req.HttpMethod == "GET")
                {
                    if (context.Request.Url.LocalPath == "/")
                        page += "index.html";


                    using (FileStream fs = File.OpenRead(page))
                    {
                        // выделяем массив для считывания данных из файла
                        byte[] buffer = new byte[fs.Length];
                        // считываем данные
                        await fs.ReadAsync(buffer, 0, buffer.Length);
                        response.ContentLength64 = buffer.Length;
                        Stream st = response.OutputStream;
                        st.Write(buffer, 0, buffer.Length);
                    }
                }
              
                context.Response.Close();
                Console.WriteLine("Closed");
            }

        }
        public static void ShowRequestData(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                Console.WriteLine("No client data was sent with the request.");
                return;
            }
            System.IO.Stream body = request.InputStream;
            System.Text.Encoding encoding = request.ContentEncoding;
            System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
            if (request.ContentType != null)
            {
                Console.WriteLine("Client data content type {0}", request.ContentType);
            }
            Console.WriteLine("Client data content length {0}", request.ContentLength64);

            Console.WriteLine("Start of client data:");
            // Convert the data to a string and display it on the console.
            string s = reader.ReadToEnd();
            Console.WriteLine(s);
            Console.WriteLine("End of client data:");
            body.Close();
            reader.Close();
            // If you are finished with the request, it should be closed also.
        }
    }
}
