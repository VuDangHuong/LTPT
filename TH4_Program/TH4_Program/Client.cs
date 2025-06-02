//using System;
//using System.Linq;
//using System.Runtime.Remoting.Channels;
//using System.Runtime.Remoting.Channels.Tcp;
//using MaxNumberService;
//using System.Net;
//using System.Net.NetworkInformation;

//namespace MaxNumberClient
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            try
//            {
//                // Lấy địa chỉ IP của máy
//                string localIP = GetLocalIPAddress();
//                Console.WriteLine($"Địa chỉ IP của máy: {localIP}");

//                // Nhập địa chỉ IP của server
//                Console.Write("Nhập địa chỉ IP của server (hoặc Enter để sử dụng localhost): ");
//                string serverIP = Console.ReadLine();
//                if (string.IsNullOrWhiteSpace(serverIP))
//                {
//                    serverIP = "127.0.0.1";
//                }

//                Console.WriteLine("Đang kết nối đến server...");
//                TcpChannel channel = new TcpChannel();
//                ChannelServices.RegisterChannel(channel, false);

//                // Thử kết nối với server
//                string serverUrl = $"tcp://{serverIP}:9000/MaxNumberService";
//                Console.WriteLine($"Đang thử kết nối đến server tại {serverUrl}");

//                IMaxNumberService service = (IMaxNumberService)Activator.GetObject(
//                    typeof(IMaxNumberService),
//                    serverUrl);

//                if (service == null)
//                {
//                    throw new Exception("Không thể kết nối đến server. Hãy kiểm tra xem server đã được khởi động chưa.");
//                }

//                int[] numbers = { 10, 5, 8, 12, 3 };
//                string[] numberStrings = numbers.Select(n => n.ToString()).ToArray();
//                Console.WriteLine("Cac so: " + string.Join(", ", numberStrings));
//                int max = service.TimSoLonNhat(numbers);
//                Console.WriteLine($"Số lớn nhất là: {max}");
//            }
//            catch (System.Net.Sockets.SocketException ex)
//            {
//                Console.WriteLine("Lỗi kết nối socket: " + ex.Message);
//                Console.WriteLine("Hãy đảm bảo:");
//                Console.WriteLine("1. Server đang chạy");
//                Console.WriteLine("2. Port 9000 không bị chặn bởi firewall");
//                Console.WriteLine("3. Không có ứng dụng khác đang sử dụng port 9000");
//                Console.WriteLine("4. Địa chỉ IP server đã nhập là chính xác");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Lỗi: " + ex.Message);
//                Console.WriteLine("Chi tiết lỗi: " + ex.ToString());
//            }
//            Console.WriteLine("\nNhấn phím bất kỳ để thoát...");
//            Console.ReadKey();
//        }

//        private static string GetLocalIPAddress()
//        {
//            var host = Dns.GetHostEntry(Dns.GetHostName());
//            foreach (var ip in host.AddressList)
//            {
//                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
//                {
//                    return ip.ToString();
//                }
//            }
//            return "127.0.0.1"; // Fallback to localhost if no IP found
//        }
//    }
//}