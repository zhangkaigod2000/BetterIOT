using System;

namespace Drive.Keyence.PLC.Nano_ASCII
{
    class Program
    {
        static void Main(string[] args)
        {
            KeyenceNanoSerialDrive keyenceNano = new KeyenceNanoSerialDrive();
            keyenceNano.Start(args[0]);
        }
    }
}
