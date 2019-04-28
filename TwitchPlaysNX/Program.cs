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

        static Dictionary<string, int> keys = new Dictionary<string, int>
        {
            { "A", 1 },
            { "B", 1 << 1 },
            { "X", 1 << 2 },
            { "Y", 1 << 3 },
            { "LST", 1 << 4 },
            { "RST", 1 << 5 },
            { "L", 1 << 6 },
            { "R", 1 << 7 },
            { "ZL", 1 << 8 },
            { "ZR", 1 << 9 },
            { "PLUS", 1 << 10 },
            { "MINUS", 1 << 11 },
            { "DL", 1 << 12 },
            { "DU", 1 << 13 },
            { "DR", 1 << 14 },
            { "DD", 1 << 15 },
            { "LL", 1 << 16 },
            { "LU", 1 << 17 },
            { "LR", 1 << 18 },
            { "LD", 1 << 19 },
            { "RL", 1 << 20 },
            { "RU", 1 << 21 },
            { "RR", 1 << 22 },
            { "RD", 1 << 23 }
        };
        static List<string[]> Commands = new List<string[]>();



        static int port = 8080;
        public static IPEndPoint ipe;
        public static Socket sock;

        static long keyInputs = 0;
        public static long keyout;
        public static int dx_l = 385;
        public static int dy_l = 643;
        public static int dx_r = 642;
        public static int dy_r = 0;




        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                Console.WriteLine("Please enter the IP address of your switch.");
                Console.WriteLine("Example: >TwitchPlaysNX <your switch IP>");
                return;
            }
            else
            {
                //Connect(args[0], port);
                Connect("10.0.0.103", port);


                Thread sendThread = new Thread(new ThreadStart(SendInput));
                Thread commandThread = new Thread(new ThreadStart(ManageCommands));
                commandThread.Start();
                sendThread.Start();

                while (true)
                {
                    string command = Console.ReadLine();

                    switch (command)
                    {
                        case "exit":
                            Disconnect();
                            sendThread.Abort();
                            return;

                        default:
                            ParseCommand(command);
                            break;
                    }


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
            while (true)
            {

                byte[] msg = new byte[26];
                byte[] msgTemp = { 0x75, 0x32 };

                msgTemp.CopyTo(msg, 0);
                BitConverter.GetBytes(keyout).CopyTo(msg, 2);
                BitConverter.GetBytes(dx_l).CopyTo(msg, 10);
                BitConverter.GetBytes(dy_l).CopyTo(msg, 14);
                BitConverter.GetBytes(dx_r).CopyTo(msg, 18);
                BitConverter.GetBytes(dy_r).CopyTo(msg, 22);

                sock.SendTo(msg, ipe);
                Thread.Sleep(16);
            }
        }

        static void ParseCommand(string command)
        {
            string[] commands = command.Split(';');
            
            for (int i = 0; i < commands.Count(); i++)
            {
                string[] args = commands[i].Split(' ');

                if(args[0] != "wait")
                {
                    if (!keys.ContainsKey(args[0]))
                    {
                        return;
                    }
                }

                Commands.Add(args);  
            }
            string[] doneArgs = { "DONE" };
            Commands.Add(doneArgs);

        }

        static void ManageCommands()
        {
            while (true)
            { 
                while (Commands.Count > 0)
                {
                    Thread.Sleep(20);
                    string[] args = Commands[0];
                    if (args[0] == "wait")
                    {
                        Commands.RemoveAt(0);
                        keyout = keyInputs; 
                        Thread.Sleep(100);

                        if (args.Count() == 2)
                        {
                            int num = int.Parse(args[1]);
                            keyInputs = 0;
                            keyout = 0;

                            if (num > 10) num = 10;

                            Thread.Sleep(num);
                        }
                        else
                            keyInputs = 0;
                            keyout = 0;
                            Thread.Sleep(1000);
                    }

                    if (args[0] == "DONE")
                    {
                        keyout = keyInputs;
                        Commands.RemoveAt(0);
                        Thread.Sleep(100);
                        keyInputs = 0;
                        keyout = 0;
                    }

                    else if (keys.ContainsKey(args[0]))
                    {
                        keyInputs |= keys[args[0]];
                        Commands.RemoveAt(0);
                    }


                }
            }
        }
    }
}
