using System;

namespace Drive.Fuji.PLC.SPBOverTcp
{
    class Program
    {
        static void Main(string[] args)
        {
            FujiSPBOverTcpDrive fujiSPBOverTcp = new FujiSPBOverTcpDrive();
            fujiSPBOverTcp.Start(args[0]);
        }
    }
}
