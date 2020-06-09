using System;

namespace Drive.LSIS.PLC.XGB_FastEnet
{
    class Program
    {
        static void Main(string[] args)
        {
            XGBFastEnetDrive fastEnetDrive = new XGBFastEnetDrive();
            fastEnetDrive.Start(args[0]);
        }
    }
}
