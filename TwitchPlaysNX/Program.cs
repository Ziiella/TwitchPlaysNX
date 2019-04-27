using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace TwitchPlaysNX
{
    class Program
    {
        public static int port;
        public static IPEndPoint ipe;
        public static Socket sock;
        public static float dx_l;
        public static float dy_l;
        public static float dx_r;
        public static float dy_r;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter the IP address of your switch.");
                Console.WriteLine("Example: >TwitchPlaysNX <your switch IP>");
                return;
            }
            else
            {
                port = 8080;
                ipe = new IPEndPoint(long.Parse(args[0]), port);
                sock = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sock.Connect(ipe);


            }
        }
    }
}
