using System;

namespace Drive.Panasonic.PLC.Mewtocol
{
    class Program
    {
        static void Main(string[] args)
        {
            PanasonicMewtocolDrive panasonic = new PanasonicMewtocolDrive();
            panasonic.Start(args[0]);
        }
    }
}
