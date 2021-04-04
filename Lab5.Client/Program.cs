using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lab5.Client
{
    class Program
    {
        static async Task Main()
        {
            var tasks = Enumerable.Range(0, 100).Select(x => Task.Run(() =>
                {
                    string host = "127.0.0.1";
                    int port = 9876;

                    using var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                    var ipAddress = IPAddress.Parse(host);
                    var endpoint = new IPEndPoint(ipAddress, port);
                    socket.Connect(endpoint);
                    Console.WriteLine($"Socket {x} connected to TCP server on {host}:{port}");

                    socket.Send(Encoding.Default.GetBytes(x.ToString()));
                    Console.WriteLine($"Sent message to server: {x}");

                    var buffer = new byte[socket.ReceiveBufferSize];
                    socket.Receive(buffer);
                    var message = Encoding.Default.GetString(buffer);
                    Console.WriteLine($"Received message from server: {message}");

                    socket.Close();
                    Console.WriteLine($"Socket {x} closed");
                })
            );
            await Task.WhenAll(tasks);
        }
    }
}
