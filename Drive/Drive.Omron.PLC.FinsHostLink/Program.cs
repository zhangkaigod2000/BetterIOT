using System;

namespace Drive.Omron.PLC.FinsHostLink
{
    class Program
    {
        static void Main(string[] args)
        {
            OmronHostLinkDrive omronHost = new OmronHostLinkDrive();
            omronHost.Start(args[0]);
        }
    }
}
