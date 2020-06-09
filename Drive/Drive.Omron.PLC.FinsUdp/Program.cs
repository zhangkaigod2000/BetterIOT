using System;

namespace Drive.Omron.PLC.FinsUdp
{
    class Program
    {
        static void Main(string[] args)
        {
            OmronFinsUdpDrive omronFins = new OmronFinsUdpDrive();
            omronFins.Start(args[0]);
        }
    }
}
