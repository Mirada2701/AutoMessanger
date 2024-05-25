using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint clientPoint = null;
            short port = 2525;
            UdpClient server = new UdpClient(port);
            while (true)
            {
                byte[] data = server.Receive(ref clientPoint);
                string message = Encoding.UTF8.GetString(data);
                Console.WriteLine($"Got message {message}   from : {clientPoint} at {DateTime.Now.ToShortTimeString()}");
                switch (message)
                {
                    case "Hello":
                    case "Hi":
                    case "Privet":
                        message = "Zdorov";
                        data = Encoding.UTF8.GetBytes(message);
                        server.SendAsync(data,data.Length,clientPoint);
                        Console.WriteLine($"Send message {message}   to : {clientPoint} at {DateTime.Now.ToShortTimeString()}");
                        break;
                    case "Paka":
                    case "Bye":
                    case "Dobranich":
                        message = "Goodbye";
                        data = Encoding.UTF8.GetBytes(message);
                        server.SendAsync(data, data.Length, clientPoint);
                        Console.WriteLine($"Send message {message}   to : {clientPoint} at {DateTime.Now.ToShortTimeString()}");
                        break;
                    default:
                        message = "Another";
                        data = Encoding.UTF8.GetBytes(message);
                        server.SendAsync(data, data.Length, clientPoint);
                        Console.WriteLine($"Send message {message}   to : {clientPoint} at {DateTime.Now.ToShortTimeString()}");
                        break;
                }
            }
        }
    }
}
