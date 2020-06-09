using System;

namespace Drive.Mitsubishi.PLC.Melsec_Serial
{
    class Program
    {
        static void Main(string[] args)
        {
            MelsecSerialDrive serialDrive = new MelsecSerialDrive();
            serialDrive.Start(args[0]);
        }
    }
}
