using System;

namespace Drive.AllenBradley.PLC.LogixTcp
{
    class Program
    {
        static void Main(string[] args)
        {
            AllenBradleyNetDrive allenBradleyNet = new AllenBradleyNetDrive();
            allenBradleyNet.Start(args[0]);
        }
    }
}
