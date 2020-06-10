using System;

namespace Drive.Keyence.PLC.Nano_SerialOverTcp
{
    class Program
    {
        static void Main(string[] args)
        {
            KeyenceNanoSerialOverTcpDrive keyenceNano = new KeyenceNanoSerialOverTcpDrive();
            keyenceNano.Start(args[0]);
        }
    }
}
