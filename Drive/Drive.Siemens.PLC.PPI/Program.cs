using System;

namespace Drive.Siemens.PLC.PPI
{
    class Program
    {
        static void Main(string[] args)
        {
            SiemensPPIDrive siemensPPI = new SiemensPPIDrive();
            siemensPPI.Start(args[0]);
        }
    }
}
