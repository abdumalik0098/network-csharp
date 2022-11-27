using System.Net;
using System.Net.Sockets;
using System.Text;

// server
IPAddress localaddress = IPAddress.Loopback;
IPEndPoint endpoint = new IPEndPoint(localaddress, 8080);
Console.WriteLine(string.Format($"Listening in port 8080 "));
Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
socket.Bind(endpoint);
socket.Listen(30);
Console.WriteLine("Сервер запущен. Ожидание подключений...\r\n");

while (true)
{
    Socket client = socket.Accept();

    string statusLine = "HTTP/1.1 200 OK\r\n";
    string responseHeader = "Content-Type: text/html\r\n";
    client.Send(Encoding.UTF8.GetBytes(statusLine));
    client.Send(Encoding.UTF8.GetBytes(responseHeader));
    client.Send(Encoding.UTF8.GetBytes("\r\n"));

    // получаем сообщение
    StringBuilder builder = new StringBuilder();
    int bytes = 0; // количество полученных байтов
    byte[] data = new byte[1024]; // буфер для получаемых данных

    do
    {
        bytes = client.Receive(data);
        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));

    }
    while (client.Available > 0);

    string getreq = builder.ToString();
    //string[] get = getreq.Split('\n');
    int index = getreq.IndexOf("/");
    var req = getreq.Substring(index + 1).Split(' ')[0];

    Console.WriteLine("Запрос: " + req);

    string path = Directory.GetCurrentDirectory() + "\\src\\";

    await Resp(req, path, client);

    // закрываем сокет
    client.Shutdown(SocketShutdown.Both);
    client.Close();
    //Console.WriteLine("Closed");
}

static async Task Resp(string req, string path, Socket client)
{
    //string responseHeader = "Content-Type: text/html\r\n";

    //client.Send(Encoding.UTF8.GetBytes(responseHeader));
    client.Send(Encoding.UTF8.GetBytes("\r\n"));

    if (req == String.Empty)
    {
        path += "index.html";
        using (FileStream fs = File.OpenRead(path))
        {
            // выделяем массив для считывания данных из файла
            byte[] buffer = new byte[1024];
            // считываем данные
            await fs.ReadAsync(buffer, 0, buffer.Length);
            var data = Encoding.UTF8.GetString(buffer);
            client.Send(Encoding.UTF8.GetBytes(data));
        }
        path = Directory.GetCurrentDirectory() + "\\src\\";
    }
    else
    {
        path += req;
        using (FileStream fs = File.OpenRead(path))
        {
            // выделяем массив для считывания данных из файла
            byte[] buffer = new byte[1024];
            // считываем данные
            await fs.ReadAsync(buffer, 0, buffer.Length);
            var data = Encoding.UTF8.GetString(buffer);
            client.Send(Encoding.UTF8.GetBytes(data));
        }
        path = Directory.GetCurrentDirectory() + "\\src\\";
    }
}