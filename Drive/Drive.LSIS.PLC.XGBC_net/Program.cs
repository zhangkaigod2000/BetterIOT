using System;

namespace Drive.LSIS.PLC.XGBC_net
{
    class Program
    {
        static void Main(string[] args)
        {
            XGBCnetDrive xGBCnet = new XGBCnetDrive();
            xGBCnet.Start(args[0]);
        }
    }
}
