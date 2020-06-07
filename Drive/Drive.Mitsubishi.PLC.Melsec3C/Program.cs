using System;

namespace Drive.Mitsubishi.PLC.Melsec3C
{
    class Program
    {
        static void Main(string[] args)
        {
            Melsec3CDrive melsec3C = new Melsec3CDrive();
            melsec3C.Start(args[0]);
        }
    }
}
