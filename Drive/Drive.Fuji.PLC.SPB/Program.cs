using System;

namespace Drive.Fuji.PLC.SPB
{
    class Program
    {
        static void Main(string[] args)
        {
            FujiSPBDrive fujiSPB = new FujiSPBDrive();
            fujiSPB.Start(args[0]);
        }
    }
}
