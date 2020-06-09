using System;

namespace Drive.Mitsubishi.PLC.MelsecR_Binary
{
    class Program
    {
        static void Main(string[] args)
        {
            MelsecRBinaryDrive melsecR = new MelsecRBinaryDrive();
            melsecR.Start(args[0]);
        }
    }
}
