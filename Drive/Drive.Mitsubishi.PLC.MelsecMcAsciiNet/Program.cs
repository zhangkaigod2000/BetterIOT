using System;

namespace Drive.Mitsubishi.PLC.MelsecMcAsciiNe
{
    class Program
    {
        static void Main(string[] args)
        {
            MelsecMcAsciiNetDrive melsecMcAsciiNet = new MelsecMcAsciiNetDrive();
            melsecMcAsciiNet.Start(args[0]);
        }
    }
}
