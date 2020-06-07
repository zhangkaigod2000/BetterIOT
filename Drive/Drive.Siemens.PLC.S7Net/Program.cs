using System;

namespace Drive.Siemens.PLC.S7Net
{
    class Program
    {
        static void Main(string[] args)
        {
            S7NetDrive s7Net = new S7NetDrive();
            s7Net.Start(args[0]);
        }
    }
}
