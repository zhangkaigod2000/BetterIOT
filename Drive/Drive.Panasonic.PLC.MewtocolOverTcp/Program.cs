using System;

namespace Drive.Panasonic.PLC.MewtocolOverTcp
{
    class Program
    {
        static void Main(string[] args)
        {
            PanasonicMewtocolOverTcpDrive panasonic = new PanasonicMewtocolOverTcpDrive();
            panasonic.Start(args[0]);
        }
    }
}
