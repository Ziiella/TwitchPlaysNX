using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
        static int port = 8080;
        public static IPEndPoint ipe;
        public static Socket sock;

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
                Connect(args[0], port);


                Thread sendThread = new Thread(new ThreadStart(SendInput));
                sendThread.Start();
                while (true)
                {

                }


            }
        }

        static void Connect(string ipAddress, int Port)
        {

            IPAddress address = IPAddress.Parse(ipAddress);
            ipe = new IPEndPoint(address, port);

            sock = new Socket(ipe.AddressFamily, SocketType.Dgram, ProtocolType.Unspecified);

            sock.Connect(ipe);

            if (sock.Connected)
            {
                Console.WriteLine("Connected!");
            }
            else
            {
                Console.WriteLine("Error!");
                Disconnect();
            }

        }

        static void Disconnect()
        {
            sock.Close();
        }

        static void SendInput()
        {
            while (true) { 
                byte[] msg = { 0x75, 0x32, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x81, 0x01, 0x00, 0x00, 0x83, 0x02, 0x00, 0x00, 0x82, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                sock.SendTo(msg, ipe);
                Thread.Sleep(16);
            }
        }


    }
}
