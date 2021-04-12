using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lab5.Client
{
    class Program
    {
        static async Task Main()
        {
            string host = "127.0.0.1";
            int port = 9876;
            var tasks = Enumerable.Range(0, 100).Select(x => Task.Run(() =>
                {
                    using var client = new TcpClient(host, port);
                    Console.WriteLine($"Client {x} connected to TCP server on {host}:{port}");

                    using var stream = client.GetStream();
                    using var writer = new StreamWriter(stream) { AutoFlush = true };
                    writer.WriteLine(x);
                    Console.WriteLine($"Sent message to server: {x}");

                    using var reader = new StreamReader(stream);
                    string message = reader.ReadLine();
                    Console.WriteLine($"Received message from server: {message}");

                    client.Close();

                    Console.WriteLine($"Connection {x} closed");
                })
            );
            await Task.WhenAll(tasks);
        }
    }
}
