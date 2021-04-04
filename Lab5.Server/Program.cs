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
        static readonly Socket listener = new(SocketType.Stream, ProtocolType.Tcp);

        static void Main()
        {
            var endpoint = new IPEndPoint(IPAddress.Any, 9876);
            listener.Bind(endpoint);
            listener.Listen();
            Console.WriteLine("Socket listening for TCP connections on port 9876\n");

            listener.BeginAccept(HandleConnection, listener);

            Console.ReadKey(true);
        }

        static void HandleConnection(IAsyncResult result)
        {
            listener.BeginAccept(HandleConnection, listener);

            using var socket = listener.EndAccept(result);
            Console.WriteLine("Socket accepted");
            
            var buffer = new byte[socket.ReceiveBufferSize];
            socket.Receive(buffer);
            var message = Encoding.Default.GetString(buffer);
            Console.WriteLine($"Message received from client: {message}");

            buffer = Encoding.Default.GetBytes(message);
            socket.Send(buffer);
            Console.WriteLine($"Message sent to client: {message}");

            socket.Close();
            Console.WriteLine($"Socket {message} disconnected\n");
        }
    }
}
