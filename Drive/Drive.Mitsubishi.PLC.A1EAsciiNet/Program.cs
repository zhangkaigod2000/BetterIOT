using System;

namespace Drive.Mitsubishi.PLC.A1EAsciiNet
{
    class Program
    {
        static void Main(string[] args)
        {
            MelsecA1EAsciiNetDrive melsecA1E = new MelsecA1EAsciiNetDrive();
            melsecA1E.Start(args[0]);
        }
    }
}
