// Server.cs
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static void Main()
    {
        // Get local IP address
        string localIP = GetLocalIPAddress();
        Console.WriteLine($"Server IP: {localIP}");

        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(localIP), 11000);
        TcpListener listener = new TcpListener(localEndPoint);

        try
        {
            listener.Start();
            Console.WriteLine("Server started. Waiting for connections...");

            while (true)
            {
                using TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine($"Client connected from: {((IPEndPoint)client.Client.RemoteEndPoint).Address}");

                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                int[] numbers = data.Split(' ')
                    .Select(int.Parse)
                    .ToArray();
                int maxNumber = numbers.Max();

                byte[] response = Encoding.ASCII.GetBytes(maxNumber.ToString());
                stream.Write(response, 0, response.Length);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
        finally
        {
            listener.Stop();
        }
    }

    static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters found");
    }
}