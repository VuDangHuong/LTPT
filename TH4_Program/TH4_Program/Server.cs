using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Net;
using System.Net.NetworkInformation;

namespace MaxNumberServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Lấy địa chỉ IP của máy
                string localIP = GetLocalIPAddress();
                Console.WriteLine($"Dia chi ip may: {localIP}");
                Console.WriteLine("dang chay khoi dong...");

                TcpChannel channel = new TcpChannel(9000);
                ChannelServices.RegisterChannel(channel, false);

                RemotingConfiguration.RegisterWellKnownServiceType(
                    typeof(MaxNumberService.MaxNumberService),
                    "MaxNumberService",
                    WellKnownObjectMode.Singleton);

                Console.WriteLine("Server khoi dong thanh cong!");
                Console.WriteLine($"Server dang lang nghe tai tcp://{localIP}:9000/MaxNumberService");
                Console.WriteLine("Nhan phim bat ki dung server");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Loi khoi dong server: " + ex.Message);
                Console.WriteLine("Chi tiet loi: " + ex.ToString());
                Console.WriteLine("\nNhan phim bat ki de thoat...");
                Console.ReadKey();
            }
        }

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1"; // Fallback to localhost if no IP found
        }
    }
}