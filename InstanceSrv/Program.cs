using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace InstanceSrv {
    internal class Program {
        private static void Main() {
            var srv = new TcpListener(IPAddress.Parse(GetLocalIpAddress()), 1337);
            srv.Start();
            Console.WriteLine("Started listening on [{0}]", GetLocalIpAddress());
            while (true) {
                if (srv.Pending()) {
                    var client = srv.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    var buffer = new byte[1];
                    Console.WriteLine("Waiting for client activity...");
                    while (true) {
                        if (!client.Connected) {
                            break;
                        }
                        var streamclient = client.GetStream();
                        streamclient.Read(buffer, 0, buffer.Length);
                        var datarecieved = Encoding.ASCII.GetString(buffer);
                        Console.Write(datarecieved);
                        //streamclient.Close();
                    }
                }
            }
        }

        public static string GetLocalIpAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)) {
                return ip.ToString();
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}