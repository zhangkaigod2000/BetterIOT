using System;

namespace Drive.AllenBradley.PLC.MicroCip
{
    class Program
    {
        static void Main(string[] args)
        {
            AllenBradleyMicroCipDrive allenBradley = new AllenBradleyMicroCipDrive();
            allenBradley.Start(args[0]);
        }
    }
}
