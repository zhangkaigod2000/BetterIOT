using System;

namespace Drive.Omron.PLC.FinsHostLinkOverTCP
{
    class Program
    {
        static void Main(string[] args)
        {
            OmronHostLinkOverTcpDrive omronHost = new OmronHostLinkOverTcpDrive();
            omronHost.Start(args[0]);
        }
    }
}
