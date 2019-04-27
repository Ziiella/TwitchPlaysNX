using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace TwitchPlaysNX
{

    /* To-Do:
        - Try to send at least one test input.
        - Add reading input from the console for testing.
        - Turn all this functionality into a Twitch bot. 
    */



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
                IPAddress address = IPAddress.Parse(args[0]);
                ipe = new IPEndPoint(address, port);

                sock = new Socket(ipe.AddressFamily, SocketType.Dgram, ProtocolType.Unspecified);

                sock.Connect(ipe);

                if (sock.Connected)
                {
                    Console.WriteLine("Connected!");
                    //string message = "<HQiiii";
                    //byte[] msg = BitConverter.GetBytes((long)message.Length) + BitConverter.GetBytes(0l) + message;
                    //sock.SendTo(msg, ipe);
                    sock.Close();
                }
                else
                {
                    Console.WriteLine("Error!");
                    sock.Close();
                }


            }
        }
    }
}
