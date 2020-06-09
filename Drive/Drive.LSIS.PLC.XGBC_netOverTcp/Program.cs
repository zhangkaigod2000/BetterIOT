using System;

namespace Drive.LSIS.PLC.XGBC_netOverTcp
{
    class Program
    {
        static void Main(string[] args)
        {
            XGBCnetOverTcpDrive xGBCnet = new XGBCnetOverTcpDrive();
            xGBCnet.Start(args[0]);
        }
    }
}
