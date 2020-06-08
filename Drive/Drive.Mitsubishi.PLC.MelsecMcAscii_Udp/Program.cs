using System;

namespace Drive.Mitsubishi.PLC.MelsecMcAscii_Udp
{
    class Program
    {
        static void Main(string[] args)
        {
            MelsecMcAsciiUdpDrive melsecMc = new MelsecMcAsciiUdpDrive();
            melsecMc.Start(args[0]);
        }
    }
}
