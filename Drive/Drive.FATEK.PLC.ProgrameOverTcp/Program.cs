using System;

namespace Drive.FATEK.PLC.ProgrameOverTcp
{
    class Program
    {
        static void Main(string[] args)
        {
            FatekProgramOverTcpDrive fatek = new FatekProgramOverTcpDrive();
            fatek.Start(args[0]);
        }
    }
}
