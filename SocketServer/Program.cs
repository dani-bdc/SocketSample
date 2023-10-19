using System.Net;
using System.Net.Sockets;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        StartServer();
    }

    private static void StartServer()
    {
        int port = 21000;
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ip = host.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ip, port);

        Thread.Sleep(10000);
        try
        {
            Socket listener = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(endPoint);
            listener.Listen();
            while (true)
            {
                Console.WriteLine("Waiting for connection");
                Socket clientSocket = listener.Accept();

                byte[] bytes = new byte[4096];
                string data = "";
                while (true)
                {
                    int numBytes = clientSocket.Receive(bytes);
                    data += Encoding.UTF8.GetString(bytes, 0, numBytes);
                    if (data.IndexOf("<EOF>") > -1)
                        break;
                }

                Console.WriteLine("Text received: {0}", data);

                byte[] message = Encoding.UTF8.GetBytes("Message from server");
                
                clientSocket.Send(message);

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        } catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}