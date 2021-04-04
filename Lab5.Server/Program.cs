using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lab5.Server
{
    class Program
    {
        static readonly TcpListener listener = new(IPAddress.Any, 9876);

        static void Main()
        {
            listener.Start();
            Console.WriteLine("Server listening for TCP on port 9876\n");

            listener.BeginAcceptTcpClient(HandleConnection, listener);

            Console.ReadKey(true);
        }

        static void HandleConnection(IAsyncResult result)
        {
            listener.BeginAcceptTcpClient(HandleConnection, listener);

            var client = listener.EndAcceptTcpClient(result);
            Console.WriteLine("Client connected");
            using var stream = client.GetStream();

            using var reader = new StreamReader(stream);
            var message = reader.ReadLine();
            Console.WriteLine($"Message received from client: {message}");

            using var writer = new StreamWriter(stream) { AutoFlush = true };
            writer.WriteLine(message);
            Console.WriteLine($"Message sent to client: {message}");

            client.Close();
            Console.WriteLine($"Client {message} disconnected\n");

            client.Dispose();
        }
    }
}
