// Client.cs
using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        try
        {
            Console.Write("Enter server IP address: ");
            string serverIP = Console.ReadLine();

            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse(serverIP), 11000);
            Console.WriteLine("Connected to server!");

            Random rand = new Random();
            int N = 8;
            int[] array = new int[N];
            for (int i = 0; i < N; i++)
            {
                array[i] = rand.Next(1, 100);
            }

            string arrayString = string.Join(" ", array);
            Console.WriteLine($"Generated array: {arrayString}");

            NetworkStream stream = client.GetStream();
            byte[] data = Encoding.ASCII.GetBytes(arrayString);
            stream.Write(data, 0, data.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string result = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            Console.WriteLine($"Maximum number: {result}");
            client.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }
}