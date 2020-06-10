using System;

namespace Drive.Keyence.PLC.MC_3E_Binary
{
    class Program
    {
        static void Main(string[] args)
        {
            KeyenceMcNetDrive keyenceMc = new KeyenceMcNetDrive();
            keyenceMc.Start(args[0]);
        }
    }
}
