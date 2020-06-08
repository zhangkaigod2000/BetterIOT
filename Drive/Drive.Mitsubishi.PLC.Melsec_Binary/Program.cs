using System;

namespace Drive.Mitsubishi.PLC.Melsec_Binary
{
    class Program
    {
        static void Main(string[] args)
        {
            MelsecBinaryDrive melsec = new MelsecBinaryDrive();
            melsec.Start(args[0]);
        }
    }
}
