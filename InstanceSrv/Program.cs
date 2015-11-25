using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace InstanceSrv
{
    class Program
    {
        static void Main(string[] args)
        {
            var srv = new TcpListener(IPAddress.Parse(GetLocalIpAddress()), 1337);
            srv.Start();
            while (true) {
                if (srv.Pending()) {
                    var client = srv.AcceptTcpClient();
                    Console.WriteLine("Connected to someone!");
                    var buffer = new byte[1];
                    while (true) {
                        NetworkStream streamclient = client.GetStream();
                        streamclient.Read(buffer, 0, buffer.Length);
                        var datarecieved = System.Text.Encoding.ASCII.GetString(buffer);
                        //datarecieved.IndexOf(.Remove();
                        Console.Write(datarecieved);
                        streamclient.Flush();
                    }
                }
            }
        }

        public static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)) {
                return ip.ToString();
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
