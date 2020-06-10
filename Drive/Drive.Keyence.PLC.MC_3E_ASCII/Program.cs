using System;

namespace Drive.Keyence.PLC.MC_3E_ASCII
{
    class Program
    {
        static void Main(string[] args)
        {
            KeyenceMcAsciiNetDrive keyenceMc = new KeyenceMcAsciiNetDrive();
            keyenceMc.Start(args[0]);
        }
    }
}
