using System;

namespace Drive.Mitsubishi.PLC.Melsec3COverTcp
{
    class Program
    {
        static void Main(string[] args)
        {
            Melsec3COverTcpDrive melsec3COverTcp = new Melsec3COverTcpDrive();
            melsec3COverTcp.Start(args[0]);
        }
    }
}
