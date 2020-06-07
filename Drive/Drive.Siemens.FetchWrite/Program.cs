using System;

namespace Drive.Siemens.PLC.FetchWrite
{
    class Program
    {
        static void Main(string[] args)
        {
            FetchWriteDrive fetchWrite = new FetchWriteDrive();
            fetchWrite.Start(args[0]);
        }
    }
}
