using System;

namespace Drive.Mitsubishi.PLC.MelsecMc_Udp
{
    class Program
    {
        static void Main(string[] args)
        {
            MelsecMcUdpDrive mcUdpDrive = new MelsecMcUdpDrive();
            mcUdpDrive.Start(args[0]);
        }
    }
}
