using System;

namespace Drive.FATEK.PLC.ProgramPort
{
    class Program
    {
        static void Main(string[] args)
        {
            FatekProgramDrive fatek = new FatekProgramDrive();
            fatek.Start(args[0]);
        }
    }
}
