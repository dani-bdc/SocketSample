using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ExecuteClient();
        }

        static void ExecuteClient()
        {
            int port = 21000;
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ip = host.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ip, port);

            Thread.Sleep(2000);
            try
            {
                Socket sender = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                sender.Connect(endPoint);
                if (sender.RemoteEndPoint != null)
                {
                    Console.WriteLine("Socket client connected to: " + sender.RemoteEndPoint.ToString());
                }

                byte[] message = Encoding.UTF8.GetBytes("Test Client<EOF>");
                int byteSend = sender.Send(message);

                byte[] messageReceived = new byte[byteSend];
                int byteReceived = sender.Receive(messageReceived);

                Console.WriteLine("Message from server: {0}", Encoding.UTF8.GetString(messageReceived, 0, byteReceived));
                
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            } 
            catch (ArgumentNullException ane)
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}