using System;

namespace Drive.Mitsubishi.PLC.A1ENetBinary
{
    class Program
    {
        static void Main(string[] args)
        {
            MelsecA1ENetDrive a1ENetDrive = new MelsecA1ENetDrive();
            a1ENetDrive.Start(args[0]);
        }
    }
}
