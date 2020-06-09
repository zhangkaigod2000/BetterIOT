using System;

namespace Drive.Omron.PLC.CipNet
{
    class Program
    {
        static void Main(string[] args)
        {
            OmronCipNetDrive omronCipNetDrive = new OmronCipNetDrive();
            omronCipNetDrive.Start(args[0]);
        }
    }
}
