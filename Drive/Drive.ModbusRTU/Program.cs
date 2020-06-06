using System;

namespace Drive.ModbusRTU
{
    class Program
    {
        static void Main(string[] args)
        {
            ModBusRTUDrive modBusRTU = new ModBusRTUDrive();
            modBusRTU.Start(args[0]);
        }
    }
}
