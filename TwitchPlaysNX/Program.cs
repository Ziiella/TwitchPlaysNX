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
        static IPAddress server_address;

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
                server_address = IPAddress.Parse(args[0]); 

            }
        }
    }
}
