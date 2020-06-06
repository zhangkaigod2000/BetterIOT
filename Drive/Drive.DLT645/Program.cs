using System;

namespace Drive.DLT645
{
    class Program
    {
        static void Main(string[] args)
        {
            DLT645Drive dLT645 = new DLT645Drive();
            dLT645.Start(args[0]);
        }
    }
}
