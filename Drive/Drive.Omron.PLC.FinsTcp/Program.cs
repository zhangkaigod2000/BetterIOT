using System;

namespace Drive.Omron.PLC.FinsTcp
{
    class Program
    {
        static void Main(string[] args)
        {
            OmronFinsNetDrive omronFins = new OmronFinsNetDrive();
            omronFins.Start(args[0]);
        }
    }
}
